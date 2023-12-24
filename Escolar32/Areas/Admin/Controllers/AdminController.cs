using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Escolar32.Context;
using Microsoft.AspNetCore.Authorization;
using Escolar32.Models;
using Microsoft.AspNetCore.Identity;
using Escolar32.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Escolar32.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager,
               AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Usuario/Cadastro
        [Authorize("Admin")]
        public IActionResult Index()
        {
            var db = _context.Alunos.Include(x=>x.Escola);

            return View(db.Where(x => x.ExAluno == false));
        }

        [Authorize(Roles = "Member,Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Alunos == null)
            {
                return NotFound();
            }

            var db = _context.Alunos.Include(x => x.Escola);
            var aluno = await db
                .FirstOrDefaultAsync(m => m.AlunoId == id);
            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);

        }

       // GET: Usuario/Cadastro/Create

        [Authorize(Roles = "Member,Admin")]
        public IActionResult Create()
        {

            var cadastro = new CadastroViewModel();

            var ListaEscolas = _context.Escolas.ToList();
            var ListaBairros = _context.Bairros.ToList();

            cadastro.ComboEscolas = new List<SelectListItem>();
            cadastro.ComboBairros = new List<SelectListItem>();

            foreach (var item in ListaEscolas)
            {
                var newItem = new SelectListItem { Value = item.EscolaId.ToString(), Text = item.EscolaNome };

                cadastro.ComboEscolas.Add(newItem);
            }

            foreach (var item in ListaBairros)
            {
                var newItem = new SelectListItem { Value = item.BairroId.ToString(), Text = item.BairroNome };

                cadastro.ComboBairros.Add(newItem);
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
                                                                "Cartorio,ValorParcela,QtdeParcelas,TotalContrato,DataCadastro,ExAluno,DataInicio, DataFim, " +
                                                                "InicioPgto, FimPgto, Pago, Jan, Fev, Mar, Abr, Mai, Jun, Jul, Ago, Set, Out, Nov, Dez")] Aluno aluno)
        {
            aluno.DataCadastro = DateTime.Now;
            aluno.TotalContrato = aluno.ValorParcela * aluno.QtdeParcelas;
            aluno.FimPgto = aluno.InicioPgto.AddMonths((aluno.QtdeParcelas-1));
                                    
            var cadastro = new CadastroViewModel();

            var ListaEscolas = _context.Escolas.ToList();
            var ListaBairros = _context.Bairros.ToList();

            cadastro.ComboEscolas = new List<SelectListItem>();
            cadastro.ComboBairros = new List<SelectListItem>();

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
                        
            var db = _context.Alunos.Include(x => x.Escola);
            var ultimo = db.OrderByDescending(m => m.AlunoId).Take(1).Last();

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
                var newItem = new SelectListItem { Value = item.EscolaId.ToString(),
                    Text = item.EscolaNome};

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
                                                                "InicioPgto, FimPgto, Pago, Jan, Fev, Mar, Abr, Mai, Jun, Jul, Ago, Set, Out, Nov, Dez")]Aluno aluno)
        {
            aluno.TotalContrato = aluno.ValorParcela * aluno.QtdeParcelas;
            aluno.FimPgto = aluno.InicioPgto.AddMonths((aluno.QtdeParcelas - 1));

            var cadastro = new CadastroViewModel();
            var ListaEscolas = _context.Escolas.ToList();
           
            cadastro.ComboEscolas = new List<SelectListItem>();
            
            foreach (var item in ListaEscolas)
            {
                var newItem = new SelectListItem { Value = item.EscolaId.ToString(),
                    Text = item.EscolaNome };

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
                return RedirectToAction(nameof(Index));
            }
            return View(cadastro);
        }

        // GET: Usuario/Cadastro/Delete/5

        [Authorize("Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Alunos == null)
            {
                return NotFound();
            }

            var db = _context.Alunos.Include(x => x.Escola);
            var aluno = await db.FirstOrDefaultAsync(m => m.AlunoId == id);
            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }

        // POST: Usuario/Cadastro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize("Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Alunos == null)
            {
                return Problem("Entity set 'AppDbContext.Alunos'  is null.");
            }
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno != null)
            {
                _context.Alunos.Remove(aluno);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("List", "ExAluno");
        }

        private bool AlunoExists(int id)
        {
            return _context.Alunos.Any(e => e.AlunoId == id);
        }
    }
}