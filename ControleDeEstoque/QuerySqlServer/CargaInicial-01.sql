DROP TABLE IF EXISTS TB_GrupoProduto;
DROP TABLE IF EXISTS TB_Usuario;



CREATE TABLE TB_Usuario 
(
	ID_Login INT IDENTITY(1,1),
	DS_Usuario VARCHAR(50) NOT NULL,
	DS_Senha VARCHAR(50) NOT NULL,
    PRIMARY KEY (ID_Login)
);

CREATE TABLE TB_GrupoProduto (

ID_GrupoProduto INT IDENTITY (1,1),
NM_GrupoProduto VARCHAR(50) NOT NULL,
DS_Ativo BIT  NOT NULL,
PRIMARY KEY (ID_GrupoProduto)

);

INSERT INTO TB_Usuario
(
DS_Usuario,
DS_Senha
)
VALUES
(
'eliezer',
'202cb962ac59075b964b07152d234b70'
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
SELECT * FROM TB_Usuario;
