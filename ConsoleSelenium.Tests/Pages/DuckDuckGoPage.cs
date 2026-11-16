using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ConsoleSelenium.Tests.Pages;

public class DuckDuckGoPage
{
    private const string UrlBase = "https://duckduckgo.com";
    private const string SeletorSugestoes = ".search-suggestions-list li, .sbox-suggestions li, [role='option']";
    private const string SeletorResultados = "a[data-testid='result-title-a']";

    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public DuckDuckGoPage(IWebDriver driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    public void Abrir() => _driver.Navigate().GoToUrl(UrlBase);

    private IWebElement CampoPesquisa() => _wait.Until(d => d.FindElement(By.Name("q")));

    public void Digitar(string termo) => CampoPesquisa().SendKeys(termo);

    public void Pesquisar(string termo)
    {
        Digitar(termo);
        CampoPesquisa().SendKeys(Keys.Enter);
        AguardarResultados();
    }

    public void AguardarResultados() =>
        _wait.Until(d => d.FindElements(By.CssSelector(SeletorResultados)).Count > 0);

    public string ClicarNaSugestao(int posicao)
    {
        var sugestoes = _wait.Until(d =>
        {
            var itens = d.FindElements(By.CssSelector(SeletorSugestoes));
            return itens.Count >= posicao ? itens : null;
        });

        var sugestao = sugestoes[posicao - 1];
        var texto = sugestao.Text.Trim();
        sugestao.Click();
        AguardarResultados();
        return texto;
    }

    public void ClicarNoPrimeiroResultado()
    {
        _driver.FindElements(By.CssSelector(SeletorResultados))[0].Click();
        _wait.Until(d => !d.Url.Contains("duckduckgo.com"));
    }

    public void Voltar()
    {
        _driver.Navigate().Back();
        AguardarResultados();
    }
}