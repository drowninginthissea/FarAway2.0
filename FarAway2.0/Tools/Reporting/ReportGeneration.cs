using Microsoft.Win32;
using System.Globalization;
using System.IO;
using Word = Microsoft.Office.Interop.Word;

namespace FarAway2._0.Tools.Reporting
{
    /// <summary>
    /// the first applicant for refactoring
    /// </summary>
    internal class ReportGeneration
    {
        #region RentContract
        public static async Task DoARentContract(ParkingSpaceRental Rent)
        {
            SaveFileDialog sv = new SaveFileDialog();
            sv.Filter = "Microsoft Word Document (*.docx)|*.docx";
            if (sv.ShowDialog() == true)
            {
                await Task.Run(() => GenerateRentContract(Rent, sv.FileName));
                MessageBox.Show("Генерация договора аренды успешно завершена!", "Генерация завершена");
            }
        }
        private static async void GenerateRentContract(ParkingSpaceRental Rent, string PathToSave)
        {
            try
            {
                byte[] FileBytes = Properties.Resources.RentContract;
                string TempFilePath = Path.GetTempFileName();
                await File.WriteAllBytesAsync(TempFilePath, FileBytes);

                Word.Application App = new Word.Application();
                App.Visible = false;
                Word.Document document = App.Documents.Open(TempFilePath);

                ChangeWordsRentContract(Rent, document);

                document.SaveAs2(FileName: PathToSave);
                document.Close();
                App.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static void ChangeWordsRentContract(ParkingSpaceRental rent, Word.Document document)
        {
            ToolsForGeneration.ReplaceWord("{CY}", $"{DateTime.Now:yyyy}", document);
            ToolsForGeneration.ReplaceWord
                ("{UserInitials}", DbUtils.GetUserInitials(rent.idUserNavigation), document);
            ToolsForGeneration.ReplaceWord
                ("{BranchAddress}", rent.idParkingSpotNavigation.idBranchNavigation.Address, document);
            ToolsForGeneration.ReplaceWord
                ("{CostPerDay}", $"{rent
                    .idParkingSpotNavigation
                    .idBranchNavigation
                    .BranchCharacteristics
                    .TheCostOfAParkingSpacePerDay:0.00}",
                document);
            ToolsForGeneration.ReplaceWord("{SD}", $"{rent.RentalStartDate:dd}", document);
            ToolsForGeneration.ReplaceWord
                ("{SM}", rent.RentalStartDate.ToString("MMMM", new CultureInfo("ru-RU")), document);
            ToolsForGeneration.ReplaceWord("{SY}", $"{rent.RentalStartDate:yyyy}", document);
            if (rent.RentEndDate == null)
            {
                ToolsForGeneration.ReplaceWord("{EndDate}", "неопределённого срока.", document);
                ToolsForGeneration.ReplaceWord("{TypeOfRentByDuration}", "тип аренды будет установлен, относительно даты окончания аренды объекта", document);
            }
            else
            {
                ToolsForGeneration.
                    ReplaceWord("{EndDate}", 
                    $"{rent.RentEndDate:dd} {rent.RentEndDate.Value.ToString("MMMM", new CultureInfo("ru-RU"))} " +
                    $"{rent.RentEndDate:yyyy} г.",
                    document);
                ToolsForGeneration.ReplaceWord("{TypeOfRentByDuration}", $"{rent.idTypeOfRentByDurationNavigation.TypeName} аренда", document);
            }
        }
        #endregion
    }
}
