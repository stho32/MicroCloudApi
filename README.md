# MicroCloudApi

The api application for the MicroCloud. The place clients and cloud-vms/computers communicate with to make everything work.

## Client Side API

The client side api uses apikeys to authenticate users. 
Every user can interact with his or her own vms. 

## Cloud Side API

The cloud side api is used by the virtual machines to communicate with the micro cloud central to tell it about e.g. the ip address it has, get the parameters the user wanted to pass to the vm and return stuff that the user wants out, e.g. build results.
