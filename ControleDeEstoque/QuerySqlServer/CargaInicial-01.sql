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
NM_Produto VARCHAR(50) NOT NULL,
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
'123'
);


INSERT INTO TB_GrupoProduto
(
NM_Produto,
DS_Senha
)
VALUES
(
'eliezer',
'123'
);