CREATE TABLE TB_Usuario 
(
	ID_Login INT IDENTITY(1,1),
	DS_Usuario VARCHAR(50) NOT NULL,
	DS_Senha VARCHAR(50) NOT NULL,
    PRIMARY KEY (ID_Login)
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