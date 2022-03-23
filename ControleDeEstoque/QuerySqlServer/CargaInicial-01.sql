DROP TABLE IF EXISTS TB_GrupoProduto;
DROP TABLE IF EXISTS TB_Usuario;
DROP TABLE IF EXISTS TB_UnidadeMedida;
DROP TABLE IF EXISTS TB_PerfilUsuario;


CREATE TABLE TB_PerfilUsuario (

	ID_PerfilUsuario INT IDENTITY (1,1) NOT NULL,
	NM_PerfilUsuario VARCHAR(50) NOT NULL,
	DS_Ativo BIT  NOT NULL,
	PRIMARY KEY (ID_PerfilUsuario)

);

CREATE TABLE TB_Usuario 
(
	ID_Usuario INT IDENTITY(1,1) NOT NULL,
	NM_Usuario VARCHAR(90) NOT NULL,
	DS_Login VARCHAR(50) NOT NULL,
	DS_Senha VARCHAR(50) NOT NULL,
	ID_PerfilUsuario INT NOT NULL,
    FOREIGN KEY (ID_PerfilUsuario) REFERENCES TB_PerfilUsuario (ID_PerfilUsuario),
    PRIMARY KEY (ID_Usuario)
);

CREATE TABLE TB_GrupoProduto (

	ID_GrupoProduto INT IDENTITY (1,1) NOT NULL,
	NM_GrupoProduto VARCHAR(50) NOT NULL,
	DS_Ativo BIT  NOT NULL,
	PRIMARY KEY (ID_GrupoProduto)

);

CREATE TABLE TB_UnidadeMedida (
	ID_UnidadeMedida INT IDENTITY (1,1) NOT NULL,
	NM_UnidadeMeDida VARCHAR(30) NOT NULL,
	SG_UnidadeMedida VARCHAR(3) NOT NULL,
	DS_Ativo BIT NOT NULL,
	PRIMARY KEY (ID_UnidadeMedida)

);


INSERT INTO TB_PerfilUsuario
(
	NM_PerfilUsuario,
	DS_Ativo

)
VALUES
(
	'Gerente',
	'true'

);

INSERT INTO TB_PerfilUsuario
(
	NM_PerfilUsuario,
	DS_Ativo

)
VALUES
(
	'Administrativo',
	'true'

);
INSERT INTO TB_PerfilUsuario
(
	NM_PerfilUsuario,
	DS_Ativo

)
VALUES
(
	'Operador',
	'true'

);

INSERT INTO TB_Usuario
(
	NM_Usuario,
	DS_Login,
	DS_Senha,
	ID_PerfilUsuario
)
VALUES
(
	'Eliezer Silva',
	'eliezer',
	'202cb962ac59075b964b07152d234b70',
	1
);

INSERT INTO TB_Usuario
(
	NM_Usuario,
	DS_Login,
	DS_Senha,
	ID_PerfilUsuario
)
VALUES
(
	'Silva',
	'helipa',
	'202cb962ac59075b964b07152d234b70',
	3
);
INSERT INTO TB_GrupoProduto
(
NM_GrupoProduto,
DS_Ativo
)
VALUES
(
'Mouses',
'true'
);


SELECT * FROM TB_GrupoProduto;
SELECT * FROM TB_UnidadeMedida;
SELECT * FROM TB_PerfilUsuario;


SELECT 
US.NM_Usuario,
US.DS_Login,
US.DS_Senha,
PF.NM_PerfilUsuario
FROM TB_Usuario AS US
INNER JOIN TB_PerfilUsuario AS PF
ON US.ID_PerfilUsuario = PF.ID_PerfilUsuario


/*

ADICIONANDO CAMPO NOT NULL EM TB EXISTENTE SEM DROP TABLE, TABELA COM REGISTROS

				CRIANDO CAMPO
ALTER TABLE TB_Usuario ADD ID_PerfilUSuario INT NULL

				CRIANDO FOREIGN KEY
ALTER TABLE TB_Usuario ADD FOREIGN KEY (ID_PerfilUSuario) REFERENCES Tb_PerfilUsuario (ID_PerfilUSuario)


		ATUALIZANDO OS REGISTROS DA TABELA USUARIO 
UPDATE TB_Usuario set ID_PerfilUsuario = 1

		ALTERANDO O A COLUNA PARA SE TORNAR NOT NULL
ALTER TABLE TB_Usuario ALTER COLUMN ID_PerfilUSuario INT NOT NULL

*/

