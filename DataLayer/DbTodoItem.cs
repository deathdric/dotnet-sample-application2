using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application2.DataLayer
{
    [Table(name:"todo_items")]
    public class DbTodoItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(name: "id")]
        public int Id { get; set; }

        [Column(name: "title")]
        public string Title { get; set; } = "";
        
        [Column(name:"is_completed")]
        public bool Completed { get; set; }
    }
}