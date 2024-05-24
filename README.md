eWay-CRM [ExecutableTriggers](https://github.com/eway-crm/triggers) are C# apps that run inside Sandbox environment. A lot of NuGet packages are not able to run in restricted environment.

You can use SandboxRunner to test whether your code will work inside restricted environment or not.

Download SandboxRunner.exe and put it's path into *Start external program* inside your project Debug properties.
Pass full path to you ExecutableTrigger binary (exe) as first parameter. All other parameters will be passed to your ExecutableTrigger binary.

```
SandboxRunner.exe MyExecutableTrigger.exe Param1 Param2
```
