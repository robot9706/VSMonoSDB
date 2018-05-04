# VSMonoSDB
A VisualStudio Mono soft debugger extension.

## Setup

Open "Tools" then "Mono SDB Settings" and set the "Mono path" to a Mono installation directory.


## Attaching
Set the project you want to debug as the Startup project.

Open "Tools" then select:
* **Debug localhost**: To connect to **localhost:[default port]**.
* **Debug remote**: To connect to a remote debug host.

Note: The MDB file will be automatically built before the debugger attaches.

## Mono commandline and embedded mono
* Command line usage: ```mono --soft-breakpoints --debugger-agent=transport=dt_socket,address=127.0.01:9000,server=y [other arguments]```
* Embedded mono: 
 
   Enabling debug mode:
   ```
   const char* options[] = {
		"--soft-breakpoints",
		"--debugger-agent=transport=dt_socket,address=192.168.0.8:9000,server=y"
	};
	mono_jit_parse_options(2, (char**)options);	
	mono_debug_init(MONO_DEBUG_FORMAT_MONO);
   ```
   
   Before loading an assembly load the MDB:
   ```
   mono_debug_open_image_from_memory(MonoImage* targetImage, mono_byte* MDBData, int MDBDataLength);
   ```
