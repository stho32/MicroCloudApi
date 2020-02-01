INSERT INTO Node (Name, RAMAvailableTotalInGB ) VALUES ( 'MASTER', 32-6)
INSERT INTO Node (Name, RAMAvailableTotalInGB ) VALUES ( 'NODE01', 32-6)
INSERT INTO Node (Name, RAMAvailableTotalInGB ) VALUES ( 'NODE02', 32-6)

INSERT INTO Configuration (Name, Value, Description)
SELECT 'VMNamesStartWith', 'MicroVM-', 'When creating VMs all their names start with this string.' UNION ALL
SELECT 'TheMastersLocalImageDirectoryBeforeDistribution', 'C:\Users\Public\Documents\Hyper-V\Virtual hard disks', 'We create disk images in the default Hyper-V directory on the master. That is the source directory for distribution of images.' UNION ALL
SELECT 'TheNodesLocalImageDirectory', 'C:\Projekte\Images', 'Every node, including the master, has this directory. Thats where we store the disk images that are the real base images for our VMS.'

