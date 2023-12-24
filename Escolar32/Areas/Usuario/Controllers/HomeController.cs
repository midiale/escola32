using Escolar32.Context;
using Escolar32.Models;
using Escolar32.Repositories.Interfaces;
using Escolar32.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Escolar32.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    [Authorize(Roles = "Member,Admin")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAlunoRepository _alunoRepository;

        public HomeController(UserManager<IdentityUser> userManager,
               AppDbContext context,IAlunoRepository alunoRepository)
        {
            _userManager = userManager;
            _context = context;
            _alunoRepository = alunoRepository;
        }

        public IActionResult Aluno()
        {
           var user = User.Identity.Name;

            var ultimo = _alunoRepository.GetAlunoByUsuario(user);
                                    
            if (ultimo != null)
            {
                return View(ultimo);
            }

            else 
                return RedirectToAction ("Create"); 
        }

        public IActionResult Create()
        {

            var cadastro = new CadastroViewModel();

            var ListaEscolas = _context.Escolas.ToList();
           
            cadastro.ComboEscolas = new List<SelectListItem>();
            
            foreach (var item in ListaEscolas)
            {
                var newItem = new SelectListItem { Value = item.EscolaId.ToString(), Text = item.EscolaNome };

                cadastro.ComboEscolas.Add(newItem);
            }
                                    
            return View(cadastro);
        }

        // POST: Usuario/Cadastro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member,Admin")]
        public async Task<IActionResult> Create(int id, [Bind("AlunoId,Nome,NomeUsuario,DataNasc,Mae,Pai,Cep,Endereco,NumeroCasa,Complemento,Bairro,Cidade,Telefone1,Telefone2,Telefone3," +
                                                                "VanAnterior,QualEscolar,EscolaId,Serie,Periodo,RespFinan,Rg,Cpf,Email,Profissao,FirmaRec," +
                                                                "Cartorio,ValorParcela,QtdeParcelas,TotalContrato,DataCadastro,ExAluno,DataFim, FimPgto")] Aluno aluno)
                                                                
        {
            aluno.DataCadastro = DateTime.Now;
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            string usuario = Convert.ToString(user.UserName);
            aluno.NomeUsuario = usuario;
            
            
            var cadastro = new CadastroViewModel();

            var ListaEscolas = _context.Escolas.ToList();
           
            cadastro.ComboEscolas = new List<SelectListItem>();
           
            foreach (var item in ListaEscolas)
            {
                var newItem = new SelectListItem { Value = item.EscolaId.ToString(), Text = item.EscolaNome };

                cadastro.ComboEscolas.Add(newItem);
            }
                  

            if (ModelState.IsValid)
            {
                _context.Add(aluno);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Review));

            }
            
            return View(cadastro);
        }

        [Authorize(Roles = "Member,Admin")]
        public IActionResult Review()
        {
            var cadastro = new CadastroViewModel();

            var ListaEscolas = _context.Escolas.ToList();
           
            cadastro.ComboEscolas = new List<SelectListItem>();
            
            foreach (var item in ListaEscolas)
            {
                var newItem = new SelectListItem { Value = item.EscolaId.ToString(), Text = item.EscolaNome };

                cadastro.ComboEscolas.Add(newItem);
            }
                        
            var ultimo = _context.Alunos.OrderByDescending(m => m.AlunoId).Take(1).Last();

            return View(ultimo);
        }

        // GET: Usuario/Cadastro/Edit/5

        [Authorize(Roles = "Member,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            var cadastro = new CadastroViewModel();
            var ListaEscolas = _context.Escolas.ToList();
            
            cadastro.ComboEscolas = new List<SelectListItem>();
            
            foreach (var item in ListaEscolas)
            {
                var newItem = new SelectListItem { Value = item.EscolaId.ToString(), Text = item.EscolaNome };

                cadastro.ComboEscolas.Add(newItem);
            }

            if (id == null || _context.Alunos == null)
            {
                return NotFound();
            }

            cadastro.Aluno = await _context.Alunos.FindAsync(id);
            if (cadastro == null)
            {
                return NotFound();
            }
            return View(cadastro);
        }

        // POST: Usuario/Cadastro/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Member,Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("AlunoId,Nome,NomeUsuario,DataNasc,Mae,Pai,Cep,Endereco,NumeroCasa,Complemento,Bairro,Cidade,Telefone1,Telefone2,Telefone3," +
                                                                "VanAnterior,QualEscolar,EscolaId,Serie,Periodo,RespFinan,Rg,Cpf,Email,Profissao,FirmaRec," +
                                                                "Cartorio,ValorParcela,QtdeParcelas,TotalContrato,DataCadastro,ExAluno,DataInicio, DataFim, " +
                                                                "InicioPgto, FimPgto, Pago, Jan, Fev, Mar, Abr, Mai, Jun, Jul, Ago, Set, Out, Nov, Dez")] Aluno aluno)
        {
            var cadastro = new CadastroViewModel();
            var ListaEscolas = _context.Escolas.ToList();
           
            cadastro.ComboEscolas = new List<SelectListItem>();
            
            foreach (var item in ListaEscolas)
            {
                var newItem = new SelectListItem { Value = item.EscolaId.ToString(), Text = item.EscolaNome };

                cadastro.ComboEscolas.Add(newItem);
            }

            if (id != aluno.AlunoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(aluno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlunoExists(aluno.AlunoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Review));
            }
            return View(cadastro);
        }

        // POST: Usuario/Cadastro/Edit/5

        [Authorize(Roles = "Member,Admin")]
        public async Task<IActionResult> Alterar(int? id)
        {
            var cadastro = new CadastroViewModel();
            var ListaEscolas = _context.Escolas.ToList();
            
            cadastro.ComboEscolas = new List<SelectListItem>();
            
            foreach (var item in ListaEscolas)
            {
                var newItem = new SelectListItem { Value = item.EscolaId.ToString(), Text = item.EscolaNome };

                cadastro.ComboEscolas.Add(newItem);
            }

            if (id == null || _context.Alunos == null)
            {
                return NotFound();
            }

            cadastro.Aluno = await _context.Alunos.FindAsync(id);
            if (cadastro == null)
            {
                return NotFound();
            }
            return View(cadastro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Member,Admin")]
        public async Task<IActionResult> Alterar(int id, [Bind("AlunoId,Nome,NomeUsuario,DataNasc,Mae,Pai,Cep,Endereco,NumeroCasa,Complemento,Bairro,Cidade,Telefone1,Telefone2,Telefone3," +
                                                                "VanAnterior,QualEscolar,EscolaId,Serie,Periodo,RespFinan,Rg,Cpf,Email,Profissao,FirmaRec," +
                                                                "Cartorio,ValorParcela,QtdeParcelas,TotalContrato,DataCadastro,ExAluno,DataInicio, DataFim, " +
                                                                "InicioPgto, FimPgto, Pago, Jan, Fev, Mar, Abr, Mai, Jun, Jul, Ago, Set, Out, Nov, Dez")] Aluno aluno)
        {
            var cadastro = new CadastroViewModel();
            var ListaEscolas = _context.Escolas.ToList();
            
            cadastro.ComboEscolas = new List<SelectListItem>();
            
            foreach (var item in ListaEscolas)
            {
                var newItem = new SelectListItem { Value = item.EscolaId.ToString(), Text = item.EscolaNome };

                cadastro.ComboEscolas.Add(newItem);
            }

            if (id != aluno.AlunoId)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(aluno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlunoExists(aluno.AlunoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Aluno));
            }
            return View(cadastro);
        }
        private bool AlunoExists(int id)
        {
            return _context.Alunos.Any(e => e.AlunoId == id);
        }
    }
}