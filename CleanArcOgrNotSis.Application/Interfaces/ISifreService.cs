namespace CleanArcOgrNotSis.Application.Interfaces;

public interface ISifreService
{
    string SifreHashle(string sifre);
    bool SifreDogrula(string sifre, string hash);
}