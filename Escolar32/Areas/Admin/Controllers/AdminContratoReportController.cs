﻿using Escolar32.Context;
using FastReport.Data;
using FastReport.Export.PdfSimple;
using FastReport.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Escolar32.Services;
using System.Globalization;

namespace Escolar32.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Member,Admin")]

public class AdminContratoReportController : Controller
{
    private readonly IWebHostEnvironment _webHostEnv;
    private readonly IConfiguration _config;
    private readonly AppDbContext _context;

    
    public AdminContratoReportController(IWebHostEnvironment webHostEnv,
        IConfiguration config, AppDbContext context)
    {
        _webHostEnv = webHostEnv;
        _config = config;
        _context = context;
    }

    string NomeDaEscola(int nomeDaEscola)
    {
        var esc = _context.Escolas.FirstOrDefault(a => a.EscolaId == nomeDaEscola);
        return esc.EscolaNome;
    }

    
    public IActionResult ContratosReport2(int Id)
    {
        var totalContrato = _context.Alunos.Include(y => y.Escola).FirstOrDefault(a => a.AlunoId == Id).TotalContrato;
        var total = Conversor.EscreverExtenso(totalContrato);
        var valorParcela = _context.Alunos.Include(y => y.Escola).FirstOrDefault(a => a.AlunoId == Id).ValorParcela;
        var parcela = Conversor.EscreverExtenso(valorParcela);
        var dataCadastro = _context.Alunos.FirstOrDefault(a => a.AlunoId == Id).DataCadastro;
        var dataFormatada = dataCadastro.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("pt-BR"));
        var webReport = new WebReport();
        webReport.Report.Load(Path.Combine(_webHostEnv.ContentRootPath,
                            "wwwroot/reports", "Contrato.frx"));

        var nomeDaEscola = _context.Alunos.FirstOrDefault(a => a.AlunoId == Id).EscolaId;
        var escolaNome = NomeDaEscola(nomeDaEscola);
        
        var mssqlDataConnection = new MsSqlDataConnection();
        mssqlDataConnection.ConnectionString =
               _config.GetConnectionString("DefaultConnection");
        var conn = mssqlDataConnection.ConnectionString;
        webReport.Report.SetParameterValue("Conn", conn);
        webReport.Report.SetParameterValue("Id", Id);
        webReport.Report.SetParameterValue("EscolaNome", escolaNome);
        webReport.Report.SetParameterValue("total", total);
        webReport.Report.SetParameterValue("Parcela", parcela);
        webReport.Report.SetParameterValue("DataCadastro", dataFormatada);

        return View(webReport);
    }
   
    [Route("ContratoPDF")]
    public IActionResult ContratoPDF(int Id)
    {
        var totalContrato = _context.Alunos.Include(y => y.Escola).FirstOrDefault(a => a.AlunoId == Id).TotalContrato;
        var total = Conversor.EscreverExtenso(totalContrato);
        var valorParcela = _context.Alunos.Include(y => y.Escola).FirstOrDefault(a => a.AlunoId == Id).ValorParcela;
        var parcela = Conversor.EscreverExtenso(valorParcela);
        var dataCadastro = _context.Alunos.FirstOrDefault(a => a.AlunoId == Id).DataCadastro;
        var dataFormatada = dataCadastro.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("pt-BR"));
        var webReport = new WebReport();
        webReport.Report.Load(Path.Combine(_webHostEnv.ContentRootPath,
                            "wwwroot/reports", "Contrato.frx"));

        var nomeDaEscola = _context.Alunos.FirstOrDefault(a => a.AlunoId == Id).EscolaId;
        var escolaNome = NomeDaEscola(nomeDaEscola);
        var mssqlDataConnection = new MsSqlDataConnection();
        mssqlDataConnection.ConnectionString =
               _config.GetConnectionString("DefaultConnection");
        var conn = mssqlDataConnection.ConnectionString;
        webReport.Report.SetParameterValue("Conn", conn);
        webReport.Report.SetParameterValue("Id", Id);
        webReport.Report.SetParameterValue("EscolaNome", escolaNome);
        webReport.Report.SetParameterValue("total", total);
        webReport.Report.SetParameterValue("Parcela", parcela);
        webReport.Report.SetParameterValue("DataCadastro", dataFormatada);

        bool v = webReport.Report.Prepare();

        Stream stream = new MemoryStream();

        webReport.Report.Export(new PDFSimpleExport(), stream);
        stream.Position = 0;

        return File(stream, "application/zip", "Contrato.pdf");
        //return new FileStreamResult(stream, "application/pdf");
    }
}