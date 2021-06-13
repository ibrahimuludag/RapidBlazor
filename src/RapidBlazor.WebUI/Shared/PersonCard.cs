using Microsoft.AspNetCore.Components;

namespace RapidBlazor.WebUI.Shared
{
    public partial class PersonCard
    {
        [Parameter] 
        public string Class { get; set; }
        [Parameter] 
        public string Style { get; set; }
    }
}
