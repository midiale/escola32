﻿using Escolar32.Models;

namespace Escolar32.Repositories.Interfaces
{
    public interface IAlunoRepository
    {
        IEnumerable<Aluno> Alunos { get; }

        IEnumerable<Aluno> ExAlunos { get; }
                
        Aluno GetAlunoById(int alunoId);

        Aluno GetAlunoByUsuario(string NomeUsuario);

    }
}
