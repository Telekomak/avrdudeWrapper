# avrdude Wrapper
Simple program that launches avrdude with arguments supplied from user-defined board presets. 

It can be used as Microchip studio (Atmel studio) tool

## Requirements
- [avrdude](https://github.com/avrdudes/avrdude/releases/tag/v7.0) must be installed on your computer
- avrdude.exe working directory must be included in PATH enviroment variable [How to?](https://www.architectryan.com/2018/03/17/add-to-the-path-on-windows-10/)

## Usage
```
>AVRDudeWrapper <source file> <preset file (optional)>
```

1. **Source file:** Compiled binary file (any format that is supported by avrdude)
2. **Preset file (optional):** json file containing presets [included here](https://github.com/Telekomak/avrdudeWrapper/blob/master/presets.json). ***If not supplied default file is used***

```
>Enter preset name:
arduino_UNO
```
Enter name of preset that you want to use

***If left empy last used preset will be used***

```
>Enter port name:
COM6
```
Enter name of port that you want to use

***If left empy last used port of the selected preset will be used***

## Custom presets
You can create custom presets simply by editing presets.json file

```json
{
  "Name":"arduino_UNO",
  "MCUName":"atmega328p",
  "Programmer":"arduino",
  "BaudRate":115200,
  "LastUsed":18,
  "Port":"COM3"
}
```

**Json properties**
1. Name: Name of the peset
2. MCUName: Name of the microcontroller
3. Programmer: What programmer to use
4. BaudRate: Communication speed to use
5. LastUsed: Property determining the last used preset - **Set this to 0**
6. Port: Communication port to use

***For properties 2 and 3 please refer to [avrdude documentation](https://www.nongnu.org/avrdude/user-manual/avrdude.html)***

## Microchip (Atmel) studio
How to use this as Microchip studio tool

**1. Open Tools menu**

![image](https://user-images.githubusercontent.com/65535357/198051581-42f5c990-267e-4564-9096-5d99d98ab91c.png)

**2. Create new external tool**

![image](https://user-images.githubusercontent.com/65535357/198052301-ae3f5735-cd58-4694-90d6-88c001556e40.png)

**Command:**
```
<AVRDudeWrapper directory>\AVRDudeWrapper.exe
```

**Arguments:**
```
$(ProjectDir)Debug\$(TargetName).a <path to presets.json>
```
***For usage as Microchip studio tool second argument must be supplied***

## Resources:
- [avrdude documentation](https://www.nongnu.org/avrdude/user-manual/avrdude.html)
