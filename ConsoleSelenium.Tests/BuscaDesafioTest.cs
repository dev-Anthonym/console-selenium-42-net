using ConsoleSelenium.Tests.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ConsoleSelenium.Tests;

[TestClass]
public class BuscaDesafioTest
{
    private IWebDriver _driver = null!;
    private DuckDuckGoPage _pagina = null!;

    [TestInitialize]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        _driver = new ChromeDriver(options);
        _pagina = new DuckDuckGoPage(_driver);
    }

    [TestCleanup]
    public void TearDown() => _driver.Quit();

    // Exercício 1
    [TestMethod]
    public void PesquisaBasica_DeveExibirTermoNoTitulo()
    {
        const string termo = "Selenium WebDriver C#";

        _pagina.Abrir();
        _pagina.Pesquisar(termo);

        Assert.IsTrue(_driver.Title.Contains(termo, StringComparison.OrdinalIgnoreCase),
            $"Título inesperado: {_driver.Title}");
    }

    // Exercício 2
    [TestMethod]
    public void Autocompletar_DeveNavegarParaTerceiraSugestao()
    {
        _pagina.Abrir();
        _pagina.Digitar("C# unit testing");

        var sugestaoEscolhida = _pagina.ClicarNaSugestao(3);

        Assert.IsTrue(_driver.Title.Contains(sugestaoEscolhida, StringComparison.OrdinalIgnoreCase),
            $"Esperava '{sugestaoEscolhida}' no título, mas veio: {_driver.Title}");
    }

    // Exercício 3
    [TestMethod]
    public void NavegacaoHistorico_DeveVoltarParaResultados()
    {
        _pagina.Abrir();
        _pagina.Pesquisar("Automated Testing");

        _pagina.ClicarNoPrimeiroResultado();
        Assert.DoesNotContain("duckduckgo.com", _driver.Url);

        _pagina.Voltar();
        Assert.Contains("duckduckgo.com", _driver.Url);
        Assert.IsTrue(_driver.Url.Contains("q=Automated", StringComparison.OrdinalIgnoreCase));
    }
}