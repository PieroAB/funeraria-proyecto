
if DB_ID('DBFUNERARIA2020')IS NOT NULL
BEGIN
USE MASTER
DROP DATABASE DBFUNERARIA2020
END
GO
CREATE DATABASE DBFUNERARIA2020
GO
USE DBFUNERARIA2020
GO
SP_HELPDB DBFUNERARIA2020
GO

set dateformat ymd
go
 
IF OBJECT_ID ('tb_genero') IS NOT NULL
DROP TABLE tb_genero
GO
create table tb_genero(
cod_gen char(1) primary key not null,
des_gen varchar(10) not null
)
go


IF OBJECT_ID ('tb_cargo') IS NOT NULL
DROP TABLE tb_cargo
GO
create table tb_cargo(
cod_car int primary key not null,
des_car varchar(20) not null
)
go

IF OBJECT_ID ('tb_estado') IS NOT NULL
DROP TABLE tb_estado
GO
create table tb_estado(
cod_est int primary key not null,
des_est varchar(10) not null
)
go


IF OBJECT_ID ('tb_distrito') IS NOT NULL
DROP TABLE tb_distrito 
GO
create table tb_distrito (
cod_dis int primary key not null,
nom_dis varchar(50) not null
)
go


IF OBJECT_ID ('tb_personal') IS NOT NULL
DROP TABLE tb_personal
GO
create table tb_personal(
dni_pers char(8) primary key not null,
nom_pers varchar(40) not null,
ape_pers varchar(40) not null,
cod_gen char(1) not null references tb_genero(cod_gen),
dir_pers varchar(80) not null,
cod_dis int not null references tb_distrito(cod_dis),
fec_nac_pers date not null,
tel_pers varchar(13) not null,
email_pers varchar(60),
cod_car int not null references tb_cargo(cod_car),
sue_pers decimal(8,2),
usu_pers varchar(15) default '',
con_pers varchar(15) default '',
cod_est int not null references tb_estado(cod_est),
img_pers varchar(80) 
)
go


IF OBJECT_ID ('tb_categoria_item') IS NOT NULL
DROP TABLE tb_categoria_item
GO
create table tb_categoria_item(
cod_cat int primary key not null,
nom_cat varchar(15) not null
)
go
  

IF OBJECT_ID ('tb_material') IS NOT NULL
DROP TABLE tb_material
GO
create table tb_material(
cod_mat char(1) primary key not null,
nom_mat varchar(20) not null
)
go


  
IF OBJECT_ID ('tb_color') IS NOT NULL
DROP TABLE tb_color
GO
create table tb_color(
cod_col char(1) primary key not null,
nom_col varchar(20) not null
);
go

IF OBJECT_ID ('tb_producto_o_servicio') IS NOT NULL
DROP TABLE tb_producto_o_servicio
GO
create table tb_producto_o_servicio(
cod_item char(7) primary key not null,  --ITEM001
nom_item varchar(50) not null,
des_item varchar(50) ,
cod_col char(1) not null references tb_color(cod_col),  
cod_mat char(1) not null references tb_material(cod_mat),
stock_item int,
pre_item decimal(8,2)not null,
img_item varchar(80) ,
cod_cat int not null references tb_categoria_item(cod_cat),
cod_est int not null references tb_estado(cod_est)
)
go
	
IF OBJECT_ID ('tb_planes') IS NOT NULL
DROP TABLE tb_planes
GO
create table tb_planes(
cod_plan char(7) primary key not null, -- PLAN001
nom_plan varchar(50) not null,
precio_total_plan decimal(8,2) not null,
img_plan varchar(80) null,
cod_est int not null references tb_estado(cod_est)
)
go

IF OBJECT_ID ('tb_det_planes') IS NOT NULL
DROP TABLE tb_det_planes
GO
create table tb_det_planes(
cod_plan char(7) not null references tb_planes(cod_plan),
cod_item char(7) not null references tb_producto_o_servicio(cod_item),
cantidad int,
primary key(cod_plan,cod_item)
)
go


IF OBJECT_ID ('tb_tipo_persona') IS NOT NULL
DROP TABLE tb_tipo_persona
GO
create table tb_tipo_persona(
cod_tipo_per int primary key not null,
des_tipo_per varchar(20) not null
)
go


IF OBJECT_ID ('tb_tipo_documento') IS NOT NULL
DROP TABLE tb_tipo_documento
GO
create table tb_tipo_documento(
cod_tipo_doc int primary key not null,
des_tipo_doc varchar(15) not null
)
go

IF OBJECT_ID ('tb_representante') IS NOT NULL
DROP TABLE tb_representante
GO
create table tb_representante (
ndoc_rep varchar(11) not null primary key,
cod_tipo_doc int not null references tb_tipo_documento(cod_tipo_doc),
cod_tipo_per int not null references tb_tipo_persona(cod_tipo_per),
nom_rep varchar(50) not null,
ape_rep varchar(50) not null,
cod_gen char(1) not null references tb_genero(cod_gen),
dir_rep varchar(80) not null,
cod_dis int not null references tb_distrito(cod_dis),
tel_rep varchar(11) not null
)
go

IF OBJECT_ID ('tb_estado_civil') IS NOT NULL
DROP TABLE tb_estado_civil
GO
create table tb_estado_civil(
cod_est_civ varchar(2) not null primary key,
des_est_civ varchar(20) not null
)
go

IF OBJECT_ID ('tb_difunto') IS NOT NULL
DROP TABLE tb_difunto
GO
create table tb_difunto(
ndoc_dif varchar(8) not null primary key,
cod_tipo_doc int not null references tb_tipo_documento(cod_tipo_doc),    --SOLO PODRA SER DNI O CE
nom_dif varchar(50) not null,
ape_dif varchar(50) not null,
cod_gen char(1) not null references tb_genero(cod_gen),
cod_est_civ varchar(2) not null references tb_estado_civil(cod_est_civ),
fec_nac_dif date not null,
fec_fall_dif date not null,
lugar_fall_dif varchar(80) not null,
img_acta_dif varchar(80) not null
)


IF OBJECT_ID ('tb_parentesco') IS NOT NULL
DROP TABLE tb_parentesco
GO
create table tb_parentesco (
cod_par int primary key not null,
des_par varchar(30) not null
)
go


IF OBJECT_ID ('tb_representante_difunto') IS NOT NULL
DROP TABLE tb_representante_difunto
GO
create table tb_representante_difunto(
ndoc_rep varchar(11) not null references tb_representante(ndoc_rep),
ndoc_dif varchar(8) not null references tb_difunto(ndoc_dif),
cod_par int not null references tb_parentesco(cod_par)
primary key(ndoc_rep,ndoc_dif)
)


IF OBJECT_ID ('tb_transacciones') IS NOT NULL
DROP TABLE tb_transacciones
GO
create table tb_transacciones (
nro_bol char(11) primary key not null, -- BOLETA00001
ndoc_rep varchar(11) not null references tb_representante(ndoc_rep),
fecha_emi_bol datetime default getdate(),
dir_sepelio varchar(80) ,
nom_cementerio varchar(50),
fec_sepelio datetime,
precio_sin_igv decimal(8,2) not null,
igv decimal(5,2) default 18,
total decimal(8,2) not null,
cod_est int not null references tb_estado(cod_est)
)
go

IF OBJECT_ID ('tb_detalle_transacciones') IS NOT NULL
DROP TABLE tb_detalle_transacciones
GO
create table tb_detalle_transacciones(
nro_bol char(11) not null references tb_transacciones(nro_bol),
cod_prod_plan char(7) not null,
cantidad  int not null,
importe decimal(8,2) not null,
subtotal decimal(8,2) not null,
primary key(nro_bol,cod_prod_plan),
constraint PLAN_DETALLE FOREIGN KEY (cod_prod_plan) REFERENCES tb_planes(cod_plan)
)
go


-- Autogenerados

-- Genera Productos/Servicios
create or alter function dbo.generaCodigoItem() returns char(7) 
as
begin
dEClare @cod char(7), @num int, @codr char(7)
	   if not exists (select * from tb_producto_o_servicio)
	      set @cod='ITEM001'
	   else
	       begin
		    select @num=cast((Substring(Max(cod_item),5,7)) as int)+1 from tb_producto_o_servicio
				set @codr=cast(@num as varchar(7))
				set @cod=concat('ITEM',Replicate('0', 3-Len(@codr))+@codr)
		   end
 return @cod
 end
 go


 select dbo.generaCodigoItem()
 go


 -- Genera Planes
create or alter function dbo.generaCodigoPlan() returns char(7) 
as
begin
dEClare @cod char(7), @num int, @codr char(7)
	   if not exists (select * from tb_planes)
	      set @cod='PLAN001'
	   else
	       begin
		    select @num=cast((Substring(Max(cod_plan),5,7)) as int)+1 from tb_planes
				set @codr=cast(@num as varchar(7))
				set @cod=concat('PLAN',Replicate('0', 3-Len(@codr))+@codr)
		   end
 return @cod
 end
 go

 select dbo.generaCodigoPlan()
 go


 --Genera Boletas
create or alter function dbo.generaBoleta() returns char(11) 
as
begin
dEClare @cod char(11), @num int, @codr char(11)
	   if not exists (select * from tb_transacciones)
	      set @cod='BOLETA00001'
	   else
	       begin
		    select @num=cast((Substring(Max(nro_bol),7,11)) as int)+1 from tb_transacciones
				set @codr=cast(@num as varchar(11))
				set @cod=concat('BOLETA',Replicate('0', 5-Len(@codr))+@codr)
		   end
 return @cod
 end
 go

 select dbo.generaBoleta()
 go

-- Insertando Datos


insert into tb_genero values('M','Masculino'), ('F','Femenino'),('O','Otro');


insert into tb_cargo values(1,'administrador(a)'),(2,'vendedor(a)'),(3,'contador(a)'),(4,'repartidor(a)'),(5,'secretario(a)');


insert into tb_estado values(1,'Activo'), (2,'Inactivo'),(3,'Emitido'),(4,'Anulado'),(5,'Cancelado');


insert tb_distrito values(1,'Ancón');
insert tb_distrito values(2,'Ate');
insert tb_distrito values(3,'Barranco');
insert tb_distrito values(4,'Bellavista');
insert tb_distrito values(5,'Breña');
insert tb_distrito values(6,'Callao');
insert tb_distrito values(7,'Carabayllo');
insert tb_distrito values(8,'Carmen de la Legua');
insert tb_distrito values(9,'Chaclacayo');
insert tb_distrito values(10,'Chorrillos');
insert tb_distrito values(11,'Cieneguilla');
insert tb_distrito values(12,'Comas');
insert tb_distrito values(13,'El Agustino');
insert tb_distrito values(14,'Independencia');
insert tb_distrito values(15,'Jesus Maria');
insert tb_distrito values(16,'La Molina');
insert tb_distrito values(17,'La Perla');
insert tb_distrito values(18,'La Punta');
insert tb_distrito values(19,'La Victoria');
insert tb_distrito values(20,'Lima');
insert tb_distrito values(21,'Lince');
insert tb_distrito values(22,'Los Olivos');
insert tb_distrito values(23,'Chosica');
insert tb_distrito values(24,'Lurin');
insert tb_distrito values(25,'Magdalena del Mar');
insert tb_distrito values(26,'Miraflores');
insert tb_distrito values(27,'Pachacámac');
insert tb_distrito values(28,'Pucusana');
insert tb_distrito values(29,'Pueblo Libre');
insert tb_distrito values(30,'Puente Piedra');
insert tb_distrito values(31,'Punta Hermosa');
insert tb_distrito values(32,'Punta Negra');
insert tb_distrito values(33,'Rimac');
insert tb_distrito values(34,'San Bartolo');
insert tb_distrito values(35,'San Borja');
insert tb_distrito values(36,'San Isidro');
insert tb_distrito values(37,'San Juan de Lurigancho');
insert tb_distrito values(38,'San Juan de Miraflores');
insert tb_distrito values(39,'San Luis');
insert tb_distrito values(40,'San Martin de Porres');
insert tb_distrito values(41,'San Miguel');
insert tb_distrito values(42,'Santa Anita');	
insert tb_distrito values(43,'Santa María del Mar');
insert tb_distrito values(44,'Santa Rosa');
insert tb_distrito values(45,'Santiago de Surco');
insert tb_distrito values(46,'Surquillo');
insert tb_distrito values(47,'Villa el Salvador');
insert tb_distrito values(48,'Villa Maria del Triunfo');
insert tb_distrito values(49,'Ventanilla');


insert into tb_personal values ('76023331','Pablo Jamiro','Urbano Tineo','M','Jiron Luis Escalar 6000 Urb. Panamericana Norte',
4,'2000-02-17','959264465','pablojamiro3003@gmail.com',1,5000.00,'Admin1234','admin1234',1,'~/Imagenes/Personal/pablo.jpg');
insert into tb_personal values ('23345215','Piero Alessandro','Bermudez Laura','M','Cal. Piedralisa 1200 HH. Las Mercedes',
10,'2000-05-20','974655116','pieroxber@outlook.com',1,4000.00,default,default,1,'~/Imagenes/Personal/piero.jpg')


insert into tb_categoria_item values(1,'Producto');
insert into tb_categoria_item values(2,'Servicio');


insert into tb_material values('-','---'),('1','Madera'),('2','Metal'),('3','Cristal');


insert into tb_color values('-','---'),('1','plateado'),('2','negro'),('3','marron'),('4','blanco'),('5','ceniza'),('6','rojo oscuro');


insert into tb_tipo_persona values (1,'Natural'),(2,'Jurídica');


insert into tb_tipo_documento values (1,'DNI'),(2,'CE'),(3,'RUC');


insert into tb_estado_civil values ('S','soltero(a)'),('C','casado(a)'),('D','divorciado(a)'), ('U','union libre'), ('SE','separado(a)'),  ('V','viudo(a)');


insert into tb_parentesco values (1,'Conyuge'),(2,'Padre'), (3,'Madre'), (4,'Hermano(a)'), (5,'Primo(a)'), (6,'Tio(a)'), (7,'Sobrino(a)'),(8,'Otros');
go


-----------------------------------------------------------------------------------------------------
-- Procedimientos almacenados para listar en los select/combobox

--1 lista de Genero
create or alter proc sp_listaGenero
as
begin
select cod_gen, des_gen from tb_genero order by des_gen asc
end
go

--2 lista de Cargos de Personal
create or alter proc sp_listaCargo
as
begin
select cod_car, des_car from tb_cargo order by des_car asc
end
go

--3 Estados
	-- lista de estados Activo y Inactivo para Actualizar el Personal
	create or alter proc sp_listaEstadoPersonal
	as
	begin
	select cod_est, des_est from tb_estado where cod_est != 3 and cod_est != 4 and cod_est!=5  order by des_est asc
	end
	go

	exec sp_listaEstadoPersonal
	go

	-- lista de estados Emitido y Anulado para Actualizar la Boleta
	create or alter proc sp_listaEstadoBoleta
	as
	begin
	select cod_est, des_est from tb_estado where cod_est != 1 and cod_est != 2 order by des_est asc
	end
	go

	exec sp_listaEstadoBoleta
	go
--4 lista de distritos
create or alter proc sp_listaDistrito
as
begin
select cod_dis, nom_dis from tb_distrito order by nom_dis asc
end
go

--5 lista de categorias del item
 create or alter proc sp_listaCategoriaItem
 as
 begin
 select cod_cat, nom_cat from tb_categoria_item order by nom_cat asc
 end
 go 

--6 lista de Materiales
create or alter proc sp_listaMaterial
as
begin
select cod_mat, nom_mat from tb_material where cod_mat!='-' order by nom_mat asc
end
go

--7 lista de Colores
create or alter proc sp_listaColor
as
begin
select cod_col, nom_col from tb_color where cod_col!='-' order by nom_col asc
end
go

--8 lista de Tipo  de Persona  'Juridica o Natural'
create or alter proc sp_listaTipoPersona 
as
begin
select cod_tipo_per, des_tipo_per from tb_tipo_persona order by des_tipo_per asc
end
go

--9 Lista de Tipo de Documento
	-- lista de tipos de documentos de identidad SOLO PARA REPRESENTANTE 'CE, DNI, RUC'
	create or alter proc sp_listaTipoDocR
	as
	begin
	select cod_tipo_doc, des_tipo_doc from tb_tipo_documento order by des_tipo_doc asc
	end
	go

	-- lista de tipos de documentos de identidad SOLO PARA DIFUNTO  'CE, DNI'
	create or alter proc sp_listaTipoDocD
	as
	begin
	select cod_tipo_doc, des_tipo_doc from tb_tipo_documento where cod_tipo_doc!=3 order by des_tipo_doc asc
	end
	go

--10 Lista de Estado Civil
create or alter proc sp_listaEstadoCivil
as
begin
select cod_est_civ, des_est_civ from tb_estado_civil order by des_est_civ asc
end
go


select*from tb_personal
go

--11 Lista de parentescos
create or alter proc sp_listaParentesco
as
begin
select cod_par, des_par from tb_parentesco order by des_par asc
end
go

---------------------------------------------------------------------------------------------------------------

-- ACCESO AL SISTEMA (personal)
 create or alter proc sp_accesoPersonal
 @usu varchar(15),
 @con varchar(15)
 as
 begin
 select * from tb_personal where usu_pers= @usu and con_pers= @con and cod_est= 1 /* 1 = Activo */
 end
 go

exec sp_accesoPersonal 'Admin1234','Admin1234'
go

----------------------------------------------------------------------------------------------------------------
-- CRUD PERSONAL
-- Insertar personal
create or alter proc sp_insertPersonal
@dni char(8),
@nom varchar(40),
@ape varchar(40),
@gen char(1),
@dir varchar(80),
@dis int,
@nac date,
@tel varchar(13),
@email varchar(60),
@car int,
@sue decimal(8,2),
@img varchar(80)
as
begin
insert into tb_personal values(@dni,@nom,@ape,@gen,@dir,@dis,@nac,@tel,@email,@car,@sue,'', '',1, @img)
end
go



-- Actualizar personal
create or alter proc sp_updatePersonal
@dni char(8),
@nom varchar(40),
@ape varchar(40),
@gen char(1),
@dir varchar(80),
@dis int,
@nac date,
@tel varchar(13),
@email varchar(60),
@car int,
@sue decimal(8,2),
@est int,
@img varchar(80)
as
begin
update tb_personal set nom_pers=@nom, ape_pers=@ape,cod_gen=@gen, dir_pers=@dir, cod_dis=@dis, fec_nac_pers=@nac, 
tel_pers=@tel, email_pers=@email, cod_car=@car, sue_pers=@sue, cod_est=@est , img_pers=@img where dni_pers=@dni
end
go


-- Eliminar personal
create or alter proc sp_deletePersonal
@dni char(8)
as
begin
delete from tb_personal where dni_pers=@dni
end
go


-- Funcion para saber si el personal tiene un usuario asignado o no
create or alter function dbo.verificarUsuario(@ndoc varchar(8)) returns varchar(11)
as
begin
declare @ver varchar(11), @usu varchar(15) 
select @usu= usu_pers from tb_personal where dni_pers=@ndoc
if (@usu='') set @ver='No asignado'
else set @ver='Asignado'
return @ver
end
go

-- Lista de personal por like
create or alter proc sp_listaPersonal
@lik varchar(40)
as
begin
select *, dbo.verificarUsuario(dni_pers) asignacion from tb_personal 
where nom_pers like concat('%',@lik,'%') or ape_pers like concat('%',@lik,'%')
order by ape_pers asc
end
go


-- Agregar Usuario y Contraseña al personal
create or alter proc sp_addUsuario
@dni char(8),
@usu varchar(15),
@con varchar(15)
as
begin
update tb_personal set usu_pers=@usu,con_pers=@con where dni_pers=@dni
end
go

/*Funcion si existe planes en una boleta*/
create or alter function dbo.existeItemPlan (@exi char(7)) returns varchar(10)
as
begin
declare @nro int=0 , @ver varchar(10)
select @nro = count(*) from tb_det_planes where cod_item= @exi
if(@nro = 0) set @ver='No Existe'
else set @ver='Existe'
return @ver
end
go


-- CRUD de items(SOLO PRODUCTOS)
-- Lista de productos
create or alter proc sp_listaProducto
as
begin
select *, dbo.existeItemPlan(cod_item) from tb_producto_o_servicio where cod_cat=1 order by nom_item asc /*  1 = Producto , 2 = Servicio*/
end
go
exec sp_listaProducto
go

-- Inserccion de productos
create or alter proc sp_insertProducto
@nom varchar(50),
@des varchar(50),
@col char(1),
@mat char(1),
@sto int,
@pre decimal(8,2),
@img varchar(80)
as
begin
insert into tb_producto_o_servicio values(dbo.generaCodigoItem(), @nom, @des, @col, @mat, @sto, @pre, @img, 1, 1)
end
go


-- Actualizacion de productos
create or alter proc sp_updateProducto
@cod char(7),
@nom varchar(50),
@des varchar(50),
@col char(1),
@mat char(1),
@sto int,
@pre decimal(8,2),
@img varchar(80),
@est int
as
begin
update tb_producto_o_servicio set nom_item=@nom, des_item=@des, cod_col=@col, cod_mat=@mat,
stock_item=@sto, pre_item=@pre, img_item=@img, cod_est=@est where cod_item = @cod
end
go

-- Eliminacion de productos
create or alter proc sp_deleteProducto
@cod char(7)
as
begin
delete from tb_producto_o_servicio where cod_item=@cod
end
go


create or alter function dbo.verificarItemPlan(@cod varchar(7)) returns varchar(9)
as
begin
declare @ver varchar(11)

if exists(select * from tb_det_planes where cod_item = @cod ) set @ver='Existe'
else set @ver='No existe'
return @ver
end
go

set dateformat ymd
go


-- CRUD de items(SERVICIOS)
-- Lista de Servicios
create or alter proc sp_listaServicio
as
begin
select *, dbo.existeItemPlan(cod_item) from tb_producto_o_servicio where cod_cat=2 order by nom_item asc /*  1 = Producto , 2 = Servicio*/
end
go

exec sp_listaServicio
go
-- Insertar Servicio
create or alter proc sp_insertServicio
@nom varchar(50),
@des varchar(50),
@pre decimal(8,2),
@img varchar(80)
as
begin
insert into tb_producto_o_servicio values(dbo.generaCodigoItem(),@nom, @des, '-','-', 0, @pre, @img, 2, 1);
end
go

--Actualizar servicio
create or alter proc sp_updateServicio
@cod char(7),
@nom varchar(50),
@des varchar(50),
@pre decimal(8,2),
@img varchar(80),
@est int
as
begin
update tb_producto_o_servicio set nom_item=@nom, des_item=@des, pre_item=@pre, img_item=@img, cod_est=@est where cod_item=@cod
end
go

-- Eliminar servicio
create or alter proc sp_deleteServicio
@cod char(7)
as
begin
delete from tb_producto_o_servicio where cod_item=@cod
end
go


--CRUD REPRESENTANTE
--Lista de representantes
create or alter proc sp_listaRepresentantes
@lik varchar(40)
as
begin 
select * from tb_representante where ape_rep like concat('%',@lik,'%') or nom_rep like concat('%', @lik,'%') order by ape_rep, nom_rep asc
end
go

-- Insertar un representante
create or alter proc sp_insertRepresentante
@ndoc varchar(11),
@tipdoc int,
@tipper int,
@nom varchar(50),
@ape varchar(50),
@gen char(1),
@dir varchar(80),
@dis int,
@tel varchar(11)
as
begin
insert into tb_representante values(@ndoc, @tipdoc, @tipper, @nom, @ape, @gen, @dir, @dis, @tel)
end
go


-- Actualizar un representante
create or alter proc sp_updateRepresentante
@ndoc varchar(11),
@tipdoc int,
@tipper int,
@nom varchar(50),
@ape varchar(50),
@gen char(1),
@dir varchar(80),
@dis int,
@tel varchar(11)
as
begin
update tb_representante set cod_tipo_doc=@tipdoc, cod_tipo_per= @tipper, nom_rep= @nom, ape_rep= @ape, cod_gen = @gen,
dir_rep= @dir, cod_dis=@dis, tel_rep=@tel  where ndoc_rep=@ndoc
end
go

-- Eliminar un representante
create or alter proc sp_deleteRepresentante
@ndoc varchar(11)
as
begin
delete from tb_representante where ndoc_rep=@ndoc
end
go


-- CRUD DIFUNTO
-- Insertar difunto
create or alter proc sp_insertDifunto
@doc varchar(8),
@tipdoc int,
@nom varchar(50),
@ape varchar(50),
@gen char(1),
@civ varchar(2),
@nac date,
@fal date,
@lug varchar(80),
@img varchar(80)
as
begin
insert into tb_difunto values(@doc,@tipdoc,@nom,@ape,@gen,@civ,@nac,@fal,@lug,@img)
end
go



-- Actualizar difunto
create or alter proc sp_updateDifunto
@doc varchar(8),
@tipdoc int,
@nom varchar(50),
@ape varchar(50),
@gen char(1),
@civ varchar(2),
@nac date,
@fal date,
@lug varchar(80),
@img varchar(80)
as
begin
update tb_difunto set cod_tipo_doc=@tipdoc, nom_dif=@nom, ape_dif=@ape, cod_gen=@gen,
cod_est_civ=@civ, fec_nac_dif=@nac, fec_fall_dif=@fal, lugar_fall_dif=@lug, img_acta_dif=@img where ndoc_dif=@doc
end
go

-- Eliminar difunto
create or alter proc sp_deleteDifunto
@doc varchar(8)
as
begin
delete from tb_difunto where ndoc_dif=@doc
end
go



-- FUNCION QUE DEVUELVE SI EL DIFUNTO TIENE UNA PERSONA ASIGNADA
create or alter function dbo.asignacionRepresentante(@ndoc varchar(8)) returns varchar(11)
as
begin
declare @nro int =0, @asig varchar(11)
select @nro=count(*) from tb_representante_difunto where ndoc_dif=@ndoc
if(@nro = 0) set @asig='No asignado'
else set @asig='Asignado'
return @asig
end
go

--Lista de difuntos por apellidos y nombres

create or alter proc sp_listaDifunto
@lik varchar(50)
as
begin
select d.*, dbo.asignacionRepresentante(ndoc_dif) Asignamiento from tb_difunto d
where ape_dif like concat('%',@lik,'%') or nom_dif like concat('%',@lik,'%')
order by ndoc_dif asc
end
go

-- combo de representantes
create or alter proc sp_listaRepresentante_Difunto
as
begin
select ndoc_rep, concat(ape_rep,', ',nom_rep) nombrecompleto from tb_representante order by(nombrecompleto) asc
end
go

-- Procedimiento para asignar un representante
create or alter proc sp_asignarRepresentante
@rep varchar(11),
@dif varchar(8),
@par int
as
begin
insert into tb_representante_difunto values(@rep,@dif,@par)
end
go

/*Funcion que devuelva si un plan tiene items o no*/
create or alter function dbo.existedetPlan (@exi char(7)) returns varchar(10)
as
begin
declare @nro int=0 , @ver varchar(10)
select @nro = count(*) from tb_det_planes where cod_plan= @exi
if(@nro = 0) set @ver='No Existen'
else set @ver='Existen'
return @ver
end
go



/*Funcion si existe planes en una boleta*/
create or alter function dbo.existeBolPlan (@exi char(7)) returns varchar(10)
as
begin
declare @nro int=0 , @ver varchar(10)
select @nro = count(*) from tb_detalle_transacciones where cod_prod_plan= @exi
if(@nro = 0) set @ver='No Existe'
else set @ver='Existen'
return @ver
end
go


create or alter proc sp_listaPlan
as
begin
select *, dbo.existedetPlan(cod_plan), dbo.existeBolPlan(cod_plan) from tb_planes  order by (cod_plan) asc
end
go

exec sp_listaPlan
go


-- Plan 
/*procedimiento para eliminar un plan con sus detalles*/

create or alter procedure sp_del_planes
@cod char(7)
as
begin
delete from tb_det_planes where cod_plan=@cod
delete from tb_planes where cod_plan=@cod
end
go


--Ver los productos agregados por cada plan
create or alter proc sp_detalleplanes
@plan char(7)
as
begin
select d.cod_plan,d.cod_item,p.nom_item,d.cantidad,(p.pre_item*d.cantidad) as importe from tb_det_planes d join tb_producto_o_servicio p on d.cod_item = p.cod_item  where cod_plan = @plan
order by d.cod_item asc
end
go

-- PARA ACTUALIZAR EL PLAN
create or alter proc sp_actualizaPlan
@cod char(7),
@nom varchar(50),
@pre Decimal(10,2),
@img varchar(80),
@est int
as
update tb_planes set nom_plan=@nom, precio_total_plan=@pre, img_plan=@img, cod_est=@est  where cod_plan=@cod
go

select*from tb_planes
go
-- Eliminar Detalle Plan
create or alter proc sp_deleteDetallePlan
@cod char(7)
as
begin
delete from tb_det_planes where cod_plan=@cod
end
go

 -- Insertar Items al actualizar el plan
create or alter proc sp_insDetallePlan
@plan char(7),
@item char(7),
@cant int
as
insert into tb_det_planes values (@plan,@item,@cant)
go



create or alter proc sp_listaItems
@nom varchar(50)
as
begin
select * from tb_producto_o_servicio where nom_item like concat('%',@nom,'%') and cod_est = 1  order by (cod_item) asc
end
go

exec sp_listaItems ''
go

use DBFUNERARIA2020

select*from tb_personal

/*Usando los procedures e insertando nuevos datos*/
exec sp_insertPersonal '75535322','Maria Mercedes','Obando Mendez','F','Cal. Miguel Grau 2942',20,'1994-07-27','983252255',
'mara0399sas@hotmail.com',3,4500.00,'~/Imagenes/Personal/maria.jpg'
exec sp_insertPersonal '02553323', 'Carmen','Sotomayor Oliva', 'F', 'Jr. Las Villas 293',15,'1995-11-30','959276583',
'carmenxyz@outlook.com',4, 4000.00,'~/Imagenes/Personal/carmen.jpg'
exec sp_insertPersonal '80923521', 'Jorge Luis','Santillan Flores', 'M', 'Av. Peru 3099',10,'1990-04-20','909313567',
'waltsanti@outlook.com',2, 3000.00,'~/Imagenes/Personal/walter.jpg'
exec sp_insertPersonal '07839566', 'Sonia','Garrido Walts', 'F', 'Jr. Peras 504',13,'1989-01-15','976022876',
'soniawalts@gmail.com',5, 4000.00,'~/Imagenes/Personal/sonia.jpg'

exec sp_addUsuario '23345215','Pierox12','12345678'

exec sp_addUsuario '02553323','repart12','12345678'

exec sp_addUsuario '80923521','vended12','12345678'

exec sp_addUsuario '07839566','secret12','12345678'

exec sp_insertRepresentante '03688554',1,2,'Juan Fernando','Rivas Rondon','M','Calle los Alisos 2004',7,'930242346'
exec sp_insertRepresentante '84662467',1,1,'Cayetana','Boganni Ramos','F','Jirón Las Amelias 1580 Cdra. 1',14,'959200535'
exec sp_insertRepresentante '78654433',1,1,'Juan Carlos','Vilca Donnato','M','Jr. Will Smith 5089',12,'999274412'
exec sp_insertRepresentante '79723251',1,1,'Sara','Perez Alarcon','F','Cal. Los Ramos Verdes 811',9,'918835322'
exec sp_insertRepresentante '89273651',1,1,'Luisa Pilar','Gutarra Manil','F','Cal. Mercedes 1092',8,'983357612'
exec sp_insertRepresentante '08127502',1,2,'Mario Edgar','Herrera Silva','M','Av. Lamark 921',6,'986515673'
exec sp_insertRepresentante '00812522',2,1,'Carl','Ugarte Lurdez','M','Av. Carlos Monge 293',1,'908821578'
exec sp_insertRepresentante '01826651',1,2,'Michaela','Suarez Vertiz','F','Av. Perla Mz B lote 2',2,'976194722'
exec sp_insertRepresentante '92761566',1,1,'Margiori','Rosales De la Banca','F','Jr. Las Malvinas 900',3,'908165926'
exec sp_insertRepresentante '40812751',1,1,'Camila','Santibanez Alcazar','M','Cal. Pershing 299',10,'902872562'

exec sp_insertDifunto '01234994', 1, 'Luis', 'Gonzales Timana', 'M','S','2000-01-10','2020-06-21', 'Hospital Cayetano Heredia', '~/Imagenes/Actas/acta001.png'
exec sp_insertDifunto '75032423', 1, 'Jose Carlos', 'Larren Isabel', 'M','V','1997-04-20','2020-08-12', 'Carretera Central km.14', '~/Imagenes/Actas/acta002.jpg'
exec sp_insertDifunto '01295223', 2,'Junior','Mendez Espinoza','M','V','1980-04-08','2019-03-28','Estacion Balta del Metropolitano','~/Imagenes/Actas/acta003.jpg'
exec sp_insertDifunto '87355225',1,'Katerine','Rodriguez Alcazar','F','D','2005-06-01','2019-12-31','En su hogar','~/Imagenes/Actas/acta4.jpg'
exec sp_insertDifunto '72351244',2,'Sonia Blade','Tineo Malca','F','S','1998-09-08','2019-03-23','Galería Las Malvinas','~/Imagenes/Actas/acta5.jpg'

exec sp_asignarRepresentante '03688554','75032423',5

exec sp_insertProducto 'ataud refinado king blanco','ataud con diseño de cristal blanco','4','2',100,1200.00,'~/Imagenes/Productos/ataudvidrioblanco.jpg'
exec sp_insertProducto 'ataud de roble clasico marron oscuro','ataud con diseño tradicional marron oscuro','3','1',300,400.00,'~/Imagenes/Productos/ataumaderaoscura.jpg'
exec sp_insertProducto 'ataud americano de metal plateado','ataud con diseño americano de metal puro','1','2',400,600.00,'~/Imagenes/Productos/ataudmamelate.jpg'

exec sp_insertServicio 'Van con rosales','-',200.00,'~/Imagenes/Servicios/vanfloral.jpg'
exec sp_insertServicio 'Conduccion e instalacion de velatorio','-',300.00,'~/Imagenes/Servicios/velatorio.jpg'
exec sp_insertServicio 'Movilidad para 6 pasajeros','-',60.00,'~/Imagenes/Servicios/auto6.jpg'
exec sp_insertServicio 'Movilidad para 10 pasajeros','-', 100.00,'~/Imagenes/Servicios/auto10.jpg'
exec sp_insertServicio 'Servicio de cargadores','-',100.00,'~/Imagenes/Servicios/cargadores.jpg'
exec sp_insertServicio 'Capilla ardiente','-',70.00,'~/Imagenes/Servicios/capilla.jpg'
exec sp_insertServicio 'Cremacion con urna','-',250.00,'~/Imagenes/Servicios/cremacion.jpg'

insert into tb_planes values ('PLAN001','Plan estandar basico',1000.00,'~/Imagenes/Planes/plan1.jpg',1)
insert into tb_planes values ('PLAN002','Plan estandar intermedio',1200.00,'~/Imagenes/Planes/plan2.png',1)
insert into tb_planes values ('PLAN003','Plan cremacion clase alta',2100.00,'~/Imagenes/Planes/plan3.jpg',1)
insert into tb_planes values ('PLAN004','Plan familiar basico',2000.00,'~/Imagenes/Planes/plancrema.jpg',2)

insert into tb_det_planes values ('PLAN001','ITEM002',1)
insert into tb_det_planes values ('PLAN001','ITEM004',1)
insert into tb_det_planes values ('PLAN001','ITEM005',1)
insert into tb_det_planes values ('PLAN001','ITEM007',1)
go

insert into tb_det_planes values ('PLAN002','ITEM003',1)
insert into tb_det_planes values ('PLAN002','ITEM004',1)
insert into tb_det_planes values ('PLAN002','ITEM005',1)
insert into tb_det_planes values ('PLAN002','ITEM007',1)
insert into tb_det_planes values ('PLAN002','ITEM008',1)
insert into tb_det_planes values ('PLAN002','ITEM009',1)
go

insert into tb_det_planes values ('PLAN003','ITEM001',1)
insert into tb_det_planes values ('PLAN003','ITEM004',1)
insert into tb_det_planes values ('PLAN003','ITEM005',1)
insert into tb_det_planes values ('PLAN003','ITEM007',1)
insert into tb_det_planes values ('PLAN003','ITEM008',1)
insert into tb_det_planes values ('PLAN003','ITEM009',1)
insert into tb_det_planes values ('PLAN003','ITEM010',1)
go

insert into tb_transacciones values 
(dbo.generaBoleta(),'84662467','2020-10-25','Av. Las Magnolias 354, Los Olivos','El Angel','2020-10-30 20:00:00',1000.00,18,1180.00,3);

insert into tb_detalle_transacciones values
('BOLETA00001','PLAN001',1,1000.00,1000.00);

insert into tb_transacciones values
(dbo.generaBoleta(),'03688554','2020-11-10','Av. Las Palmeras 5090, La Molina','Casimiro Loayza','2020-11-17 13:30:00',2100.00,18,2478.00,5);
insert into tb_detalle_transacciones values
('BOLETA00002','PLAN003',1,2100.00,2100.00);
go

insert into tb_transacciones values 
(dbo.generaBoleta(),'03688554','2020-11-07','Cal. Perching 0392, San Isidro','El Angel','2020-11-09 14:00:00',1200.00,18,1416.00,3);

insert into tb_detalle_transacciones values
('BOLETA00003','PLAN002',1,1200.00,1200.00);
go

insert into tb_transacciones values 
(dbo.generaBoleta(),'40812751','2020-11-16','Cal. Delicias 102, Villa Salvador','Chullpa','2020-11-18 14:00:00',1200.00,18,1416.00,4);

insert into tb_detalle_transacciones values
('BOLETA00004','PLAN002',1,1200.00,1200.00);
go


insert into tb_transacciones values 
(dbo.generaBoleta(),'40812751','2020-11-18','Av. Merino Mz.4 lote 433, Puente Piedra','Chauchilla','2020-11-20 09:00:00',1200.00,18,1416.00,5);

insert into tb_detalle_transacciones values
('BOLETA00005','PLAN002',1,1200.00,1200.00);
go

insert into tb_transacciones values 
(dbo.generaBoleta(),'84662467','2020-11-20','Av. Carlos Izaguirre cdr.4, Jesus Maria','El Angel','2020-11-22 14:00:00',2100.00,18,2478.00,3);

insert into tb_detalle_transacciones values
('BOLETA00006','PLAN003',1,1200.00,1200.00);
go

insert into tb_transacciones values 
(dbo.generaBoleta(),'89273651','2020-11-28','Cal. Delicias 102, Villa Salvador','Campo  Fe','2020-12-01 14:00:00',1000.00,18,1180.00,3);

insert into tb_detalle_transacciones values
('BOLETA00007','PLAN001',1,1000.00,1000.00);
go

insert into tb_transacciones values 
(dbo.generaBoleta(),'03688554','2020-11-30','Cal. Calderon 309, Barrios Altos','La Almudena','2020-12-02 18:00:00',1200.00,18,1416.00,3);

insert into tb_detalle_transacciones values
('BOLETA00008','PLAN002',1,1200.00,1200.00);
go


select*from tb_representante

select*from tb_planes
go

create or alter proc sp_insertDetalleTransaccion
@bol char(11),
@plan char(7),
@can  int ,
@imp decimal(8,2),
@sub decimal(8,2)
as
begin
insert into tb_detalle_transacciones values(@bol,@plan,@can,@imp,@sub)
end
go

create or alter proc sp_createTransaccion
@nb char(11),
@rep varchar(11),
@dir varchar(80) ,
@cem varchar(50) ,
@fs datetime,
@sub decimal(8,2),
@total decimal(8,2) 

as
begin
insert tb_transacciones values(@nb,@rep,default,@dir,@cem,@fs,@sub,18,@total,3)
end
go

/*consultar boletas entre fechas*/

-- Transacciones / Boleta

create or alter proc sp_listaBoletas
as
begin
    select t.nro_bol,t.fecha_emi_bol,t.ndoc_rep,t.dir_sepelio,t.nom_cementerio,t.fec_sepelio,t.precio_sin_igv,t.total,t.cod_est,e.des_est from tb_transacciones t
    inner join tb_estado e on e.cod_est= t.cod_est
	order by t.nro_bol desc, t.fecha_emi_bol desc
end
go

create or alter proc sp_listafechaBoletas
@f1 date,
@f2 date
as
begin
if(@f1 is null or @f2 is null)
begin
  select t.nro_bol,t.fecha_emi_bol,t.ndoc_rep,t.dir_sepelio,t.nom_cementerio,t.fec_sepelio,t.precio_sin_igv,t.total,e.des_est from tb_transacciones t
  inner join tb_estado e on e.cod_est= t.cod_est
  order by t.nro_bol desc, t.fecha_emi_bol desc
end
else 
   begin
    select t.nro_bol,t.fecha_emi_bol,t.ndoc_rep,t.dir_sepelio,t.nom_cementerio,t.fec_sepelio,t.precio_sin_igv,t.total,t.cod_est,e.des_est from tb_transacciones t
    inner join tb_estado e on e.cod_est= t.cod_est
    where t.fecha_emi_bol between @f1 and @f2
	order by fecha_emi_bol desc, t.cod_est asc
   end
end
go

/*COMBO DE REPRESENTANTES*/
create or alter proc sp_comboRepresentantes
as
begin
select ndoc_rep, concat(ape_rep,', ',nom_rep) from tb_representante
order by ape_rep desc, nom_rep asc
end
go


/*EDITA LA BOLETA*/
create or alter proc sp_editaBoleta
@cod char(11),
@dir varchar(80),
@cem varchar(50),
@fsep datetime,
@est int
as
begin
update tb_transacciones set dir_sepelio=@dir, nom_cementerio=@cem, fec_sepelio= @fsep, cod_est=@est where nro_bol=@cod 
end
go


/*ANULA LA BOLETA*/
create or alter proc sp_anulaBoleta
@cod char(11)
as
begin
update tb_transacciones set cod_est = 4 where nro_bol=@cod
end
go

/*DETALLE DEL PLAN DE LA BOLETA*/
create or alter procedure sp_lista_det_plan_boleta
@cod char(11)
as
begin
select ps.cod_item,ps.nom_item, ps.des_item,sum(dt.cantidad) cantidad
from tb_detalle_transacciones dt
inner join tb_transacciones t on t.nro_bol= dt.nro_bol
inner join tb_planes p on dt.cod_prod_plan= p.cod_plan
inner join tb_det_planes dp on dp.cod_plan=p.cod_plan
inner join tb_producto_o_servicio ps on dp.cod_item= ps.cod_item
where t.nro_bol=@cod
group by ps.cod_item, ps.nom_item, ps.des_item
order by ps.cod_item asc
end
go

exec sp_lista_det_plan_boleta 'BOLETA00004'

