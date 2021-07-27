using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryForce.Shared.Models;

namespace StoryForce.Shared.Dtos
{
    public class PeopleDto
    {
        public Person[] People { get; set; }

        public static IEnumerable<PeopleSelect2Vm> ConvertFromEntityToSelect2Vm(Person[] entity)
        {
            foreach (var item in entity)
            {
                yield return new PeopleSelect2Vm
                {
                    Id = item.Id,
                    Text = !string.IsNullOrEmpty(item.Name) ? item.Name : (item.Email ?? string.Empty)
                };
            }
        }

    }

    public class PeopleSelect2Vm
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
