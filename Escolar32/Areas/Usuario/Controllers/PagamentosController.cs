﻿using Escolar32.Context;
using Escolar32.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Escolar32.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    [Authorize(Roles = "Member,Admin")]
    public class PagamentosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAlunoRepository _alunoRepository;

        public PagamentosController(UserManager<IdentityUser> userManager,
               AppDbContext context, IAlunoRepository alunoRepository)
        {
            _userManager = userManager;
            _context = context;
            _alunoRepository = alunoRepository;
        }
        public IActionResult Pagamentos()
        {
            var user = User.Identity.Name;

            var ultimo = _alunoRepository.GetAlunoByUsuario(user);

            if (ultimo != null)
            {
                return View(ultimo);
            }

            else
                return RedirectToAction("Index", "Home");
        }
    }
}
