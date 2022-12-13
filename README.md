# AccioInventory
Welcome to Accio open source inventory system written to simplify all store needs. </br></br></br>
<p align="center">
<a href="https://imgbb.com/" align="center"><img src="https://i.ibb.co/7S1fgjB/acci-400x400-1.jpg" alt="acci-400x400-1" style="border-radius:54px; width: 115px;  display: block;
  margin-left: auto;
  margin-right: auto;"></a>
  </p>
</br>

<!-- ABOUT THE PROJECT -->
## About The Project: 💼</br>
* This idea is not new but, Many want to implement web based systems in desktop application actually in windows env. for more speed and reliability</br>here we use C sharp as a main lang with some dlls and tweaks.

</br></br>

<!-- ABOUT THE PROJECT -->
## Database Version: 💉</br>
* Here we use <a href="https://en.wikipedia.org/wiki/Oracle_Database">Oracle</a> Database through Oracle with (ManagedDataAccess.dll) this library can be used as 
a connection object and execute all sql commands as string.

</br>

<!-- SCREENS -->
## Screens: 📷</br>

* Login.. </br>
<a href="https://ibb.co/fXKxrHT"><img src="https://i.ibb.co/kGrSqXP/Screenshot-64.png" alt="Screenshot-64" ></a>
* Users page.. </br>
<a href="https://ibb.co/mqWyVnH"><img src="https://i.ibb.co/QNwH3W6/Screenshot-70.png" alt="Screenshot-70" border="0"></a>

<!-- VIDEO -->
## Video: 📹</br>
<div>
<a href="https://www.youtube.com/embed/r95LhQzBCRA">Go to youtube 👉</a> 

    
</div>

 
</br>

What you need to know:.🫡 </br>
#1 this SW using database managed dll for oracle version 10g.<br>
#2 you should know how to build all tables to act inside scripts.<br>
#3 working in adding .cs file to handle main SQL commands and quiries.<br>
</br></br></br>
Very Impertant!</br>
#1 first thing first you must build your database.</br>
#2 I will put down the structure script you may need to change it.</br>
<a href="https://lucid.app/lucidchart/0cc752d3-dca9-4cbb-97de-b78184f47b96/edit?viewport_loc=21%2C208%2C1755%2C844%2C0_0&invitationId=inv_2c1a550c-2bc2-4136-9aea-5f2b7bbd4ec2">🫳Link to ERD</a></br>
#3 there is a file called 'params.info' its for intit the current connection.</br>
<a href="https://imgbb.com/"><img src="https://i.ibb.co/ZmY8D0W/expl.png" alt="expl" border="0"></a>
</br>
params.info :
````
 
**this file is to initialize the current database connnection to make app connect to its server**
**don't try to move lines down or up this will miss the whole file just change data values**
[
#server_ip::127.0.0.1,
#port::1521,
#company_name::Haam Corporation,
#version::1.1.0,
]

````
<!-- VIDEO -->
## To Do: ⛏️🤦</br>
- [x] Add params file in the project.
- [ ] Convert Scripts.cs to .dll library.