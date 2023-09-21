namespace CSVDBService;
using System.Numerics;
using System.Text.RegularExpressions;
using CsvHelper;
using System.Collections;
using System.ComponentModel;
using CsvHelper.Configuration;
using System.Globalization;




public interface IDatabaseRepository<T>
{
    public IEnumerable<T> Read(int? limit = null);
    public void Store(T record);
}