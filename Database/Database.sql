/* 
	This scripts creates or recreates the database.
*/

/* ------------------------------------------------------------------------------------------

	For every node in our system we have an entry in this table. 

*/	
 
IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'VirtualMachine'))
BEGIN
	DROP TABLE VirtualMachine;
END
GO

IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'Node'))
BEGIN
	DROP TABLE Node;
END
GO

CREATE TABLE Node (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name VARCHAR(200) NOT NULL,
	RAMAvailableTotalInGB DECIMAL(15,2) NOT NULL DEFAULT 0,
	IsActive BIT NOT NULL DEFAULT 1
)
GO

/* ------------------------------------------------------------------------------------------

	General configuration is kept in the database as well

*/	

IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'Configuration'))
BEGIN
	DROP TABLE Configuration;
END
GO

GO
CREATE TABLE Configuration (
	Id INT NOT NULL PRIMARY KEY IDENTITY, 
	Name VARCHAR(200) NOT NULL DEFAULT '',
	Value VARCHAR(1000) NOT NULL DEFAULT '',
	Description VARCHAR(2000) NOT NULL DEFAULT ''
)
GO


/* ------------------------------------------------------------------------------------------

	We have a list of disk images, that are available on the master in a separate
	path. (The master is a node, just one with more tasks to perform.)

*/	

IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'DiskImage'))
BEGIN
	DROP TABLE DiskImage;
END
GO

CREATE TABLE DiskImage (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name VARCHAR(200) NOT NULL,
	SourcePathOnMaster VARCHAR(1000) NOT NULL DEFAULT '',
	IsActive BIT NOT NULL DEFAULT 1
)
GO

/* ------------------------------------------------------------------------------------------

	All of those disk images have to be distributed to all nodes.

	(later)
*/	

/* ------------------------------------------------------------------------------------------

	At last, we have virtual machines. 
	Every row in this table is actually an "intent" that there should be 

*/	


CREATE TABLE VirtualMachine (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name VARCHAR(200) NOT NULL DEFAULT '',
	BaseImage VARCHAR(200) NOT NULL DEFAULT '',
	CreatedOnNode INT NOT NULL REFERENCES Node(Id),

	Alias VARCHAR(200) NOT NULL DEFAULT '',
	Status VARCHAR(200) NOT NULL DEFAULT '',
	RAMinGB DECIMAL(15,4) NOT NULL DEFAULT 4,
	CloudInternalIP VARCHAR(200) NOT NULL DEFAULT '',
	CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

	ActivateThisVm BIT NOT NULL DEFAULT 0,
	IsActivated BIT NOT NULL DEFAULT 0,
	RemoveThisVm BIT NOT NULL DEFAULT 0
)

/* ------------------------------------------------------------------------------------------

	We declare port forwardings for every virtual machine.
	There is a background process on the master that takes the contents of this
	table and communicates with the router to ensure everyone
	has all the ports available they need.

	When a virtual machine is deactivated, all port forwardings are deleted.

*/	
IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'VirtualMachinePortForwarding'))
BEGIN
	DROP TABLE VirtualMachinePortForwarding;
END
GO

CREATE TABLE VirtualMachinePortForwarding (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Comment VARCHAR(200) NOT NULL DEFAULT '',
	LocalPort INT NOT NULL DEFAULT 0,
	PortOnEntranceRouter INT NOT NULL DEFAULT 0,
	IsEnabled BIT NOT NULL DEFAULT 0 -- Has it been processed and set on the router? (The background process does that)
)

GO

/* ------------------------------------------------------------------------------------------

*/	
IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'RAMOccupiedPerNode'))
BEGIN
	DROP VIEW RAMOccupiedPerNode;
END
GO
CREATE VIEW RAMOccupiedPerNode
    AS
	/* 
		sums up the ram used per node
	 */

	SELECT CreatedOnNode, SUM(RAMInGB) AS RAMOccupiedInGB
	  FROM VirtualMachine v
     GROUP BY CreatedOnNode 
GO


/* ------------------------------------------------------------------------------------------

*/	
IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'MICRONodeStats'))
BEGIN
	DROP VIEW MICRONodeStats;
END
GO
CREATE VIEW MICRONodeStats
    AS
	/* This view returns a statistic about the available RAM on each node
	   by doing a simple database query.

	   For the advantages and disadvantages of the approach see the help of the cmdlet
	   with the same name.
	 */
	 SELECT Id,
			Name, 
			RAMAvailableTotalInGB - ISNULL(RAMOccupiedInGB,0) AS RamTotalGB
	   FROM Node n
	   LEFT JOIN RAMOccupiedPerNode ropn ON ropn.CreatedOnNode = n.Id
	  WHERE n.IsActive = 1
GO

/* ------------------------------------------------------------------------------------------

	Create a virtual machine

*/	
IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME LIKE 'AddMicroVM'))
BEGIN
	DROP PROCEDURE AddMicroVM;
END
GO

CREATE PROCEDURE AddMicroVM (@BaseImage VARCHAR(200))
    AS
BEGIN
	/* Creates a virtual machine */
	DECLARE @Node INT
	DECLARE @Name VARCHAR(200)
	DECLARE @NewId INT
	
	/* Get the node with most RAM available */
	SELECT TOP 1 @Node = Id
	  FROM MICRONodeStats
	 WHERE RamTotalGB > 4
	 ORDER BY RamTotalGB DESC

	/* Grab a new name */
	SELECT @Name = (SELECT Value FROM Configuration WHERE Name = 'VMNamesStartWith') + CAST(NEWID () AS VARCHAR(100))
	
	INSERT INTO VirtualMachine (Name, BaseImage, CreatedOnNode, ActivateThisVm)
	SELECT @Name, @BaseImage, @Node, 1

	SELECT @Name AS Name
END
GO
/* ------------------------------------------------------------------------------------------

	View to select all vms that need to be added in this round

*/	
IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'VirtualMachinesThatWaitForActivation'))
BEGIN
	DROP VIEW VirtualMachinesThatWaitForActivation;
END
GO
CREATE VIEW VirtualMachinesThatWaitForActivation
	AS
	/* these are the virtual machines that we need to "activate"
	   which basically means we create them within the hypervisor.
	*/
	SELECT vm.Name AS VMName, 
		   n.Name AS Node,
		   BaseImage
	  FROM VirtualMachine vm
	  JOIN Node n ON vm.CreatedOnNode = n.Id
	 WHERE ActivateThisVm = 1
	   AND IsActivated = 0
GO

/* ------------------------------------------------------------------------------------------

	View to select all vms that need to be added in this round

*/	
IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'VirtualMachinesThatWaitForRemoval'))
BEGIN
	DROP VIEW VirtualMachinesThatWaitForRemoval;
END
GO
CREATE VIEW VirtualMachinesThatWaitForRemoval
	AS
	/* these are the virtual machines that we need to "activate"
	   which basically means we create them within the hypervisor.
	*/
	SELECT vm.[Name] AS VMName, 
		   n.Name AS Node
	  FROM VirtualMachine vm
	  JOIN Node n ON vm.CreatedOnNode = n.Id
	 WHERE RemoveThisVm = 1
GO


/* ------------------------------------------------------------------------------------------

	The virtual machine has been activated

*/	
IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME LIKE 'VmHasBeenActivated'))
BEGIN
	DROP PROCEDURE VmHasBeenActivated;
END
GO

CREATE PROCEDURE VmHasBeenActivated (@Name VARCHAR(200))
    AS
BEGIN
	/*
		This STP is called when the vm has been created and started on the hyper-visor.
		We need to update our data now, so we know it has been activated.
	*/
	UPDATE dbo.VirtualMachine
	   SET IsActivated = 1
	 WHERE Name = @Name
END
