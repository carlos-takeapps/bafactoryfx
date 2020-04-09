1- use this website as the base for your own website
2- your website could be an area, add a new area (or many) to content your site pages if you wish
3- edit Global.asax.cs and add the proper namespace so your default HomeController and Index actions could be identified by the system.

A) Construir sobre el website

En este caso basta con agregar las 


B) Referenciar el website y la dll

1) crear un mvc3
2) agregar los proyectos de la librería y el website a la solución
3) Compilar y publicar el website
4) sobre el sitio mvc3 creado en 1), copiar y pegar el contenido de la carpeta en la que se publicó el website
5) corregir la clase de la que se hereda en el archivo global.asax
6) reemplazar el global.asax.cs del sitio creado por el del website y corregir el namespace de defaul map route
7) modificar la herencia de las clases AccountController.cs, AccountModel.cs y modificar la AccountView.cs del sitio creado por el del website
8) agregar la referencia a la librería System.Data.Entity (copiar localmente)

