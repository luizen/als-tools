using Spectre.Console;

namespace AlsTools.Ui.Cli
{
    public class ConsolePrinter
    {
        public async Task PrintResults<T>(IEnumerable<T> items, string title, string[] columnNames, Func<T, string[]> getColumnValues, bool wrap = true, bool expand = true)
        {
            var table = CreateSimpleConsoleTable(title, columnNames, wrap, expand);

            await AddRowsToTable(table, items.ToArray(), getColumnValues);

            await Task.Run(() => AnsiConsole.Write(table));
        }

        public async Task PrintResults<T>(T value, string title, string[] columnNames, Func<T, string[]> getColumnValues, bool wrap = true, bool expand = true)
        {
            var table = CreateSimpleConsoleTable(title, columnNames, wrap, expand);

            await AddRowsToTable(table, [value], getColumnValues);

            await Task.Run(() => AnsiConsole.Write(table));
        }

        private Table CreateSimpleConsoleTable(string title, string[] columnNames, bool wrap = true, bool expand = true)
        {
            var paddedTitle = $" {title} ";
            var tableTitle = new TableTitle(paddedTitle, new Style(foreground: Color.Black, background: Color.SkyBlue1));
            var table = new Table().Title(tableTitle).Border(TableBorder.Rounded).BorderColor(Color.SkyBlue1);

            if (expand)
                table.Expand();

            table.AddColumn("#");

            foreach (var columnName in columnNames)
            {
                table.AddColumn(columnName, wrap ? null : c => c.NoWrap());
            }

            return table;
        }

        private async Task AddRowsToTable<T>(Table table, T[] items, Func<T, string[]> getColumnValues)
        {
            int rowNumber = 1;

            foreach (var item in items)
            {
                var columnValues = getColumnValues(item);

                List<string> rowColumns = [rowNumber.ToString(), .. columnValues];

                var texts = rowColumns.Select(v => new Text(v)).ToArray();

                await Task.Run(() => table.AddRow(texts));

                rowNumber++;
            }
        }
    }
}