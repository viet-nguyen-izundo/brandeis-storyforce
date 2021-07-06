using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using StoryForce.Server.Services;

namespace StoryForce.Server.Pages.Admin
{
    public class ImportUserModel : PageModel
    {
        private readonly IPeopleService _peopleService;

        public ImportUserModel(IPeopleService peopleService)
        {
            this._peopleService = peopleService;            
        }
        public async Task<ActionResult> OnPostAsync(IFormFile file)
        {
            var list = new List<StoryForce.Shared.Models.Person>();
            if (file != null)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var rowcount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowcount; row++)
                        {
                            list.Add(new StoryForce.Shared.Models.Person
                            {
                                Name = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                Email = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                ClassOfYear = Int32.Parse(worksheet.Cells[row, 3].Value.ToString()),
                                PhoneNumber = worksheet.Cells[row, 4].Value.ToString().Trim(),
                                UserName = worksheet.Cells[row, 5].Value.ToString().Trim(),
                            });
                        }
                    }
                }
                foreach (var item in list)
                {
                    var per = await _peopleService.GetByEmailAsync(item.Email);
                    if (per != null)
                    {
                        per.Name = item.Name;
                        per.Email = item.Email;
                        per.ClassOfYear = item.ClassOfYear;
                        per.PhoneNumber = item.PhoneNumber;
                        per.UserName = item.UserName;
                        await _peopleService.UpdateAsync(per.Id, per);
                    }
                    else
                    {
                        await _peopleService.CreateAsync(item);
                    }
                }
            }
            return Redirect("/usermanagement");
        }
    }
}
