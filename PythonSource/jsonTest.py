# -*- coding: UTF-8 -*-
import json

class Pessoa:
    nome = ""
    idade = 0
    sexo = ""
    estadoCivil = ""

    def __init__(self, nome, idade, sexo, estadoCivil):
        self.nome = nome
        self.idade = idade
        self.sexo = sexo
        self.estadoCivil = estadoCivil

if "entradaJson" not in locals():
    entradaJson = '{"primeiroNome": "Matheus", "ultimoNome": "Oliveira dos Santos", "idade": 22, "sexo": "M", "estCiv": "Solteiro"}'            

jsonEnt = json.loads(entradaJson)
nome = jsonEnt["primeiroNome"] + " " + jsonEnt["ultimoNome"]
idade = jsonEnt["idade"]
sexo = "Masculino" if(jsonEnt["sexo"] == "M")  else "Feminino"
estadoCivil = jsonEnt["estCiv"]

ret = Pessoa(nome,idade,sexo,estadoCivil)

saida = json.dumps(ret.__dict__,ensure_ascii=False)