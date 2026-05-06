using System.ComponentModel.DataAnnotations;

namespace hifi_Infrastructure.ViewModel
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Введіть ім'я")]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введіть email")]
        [EmailAddress(ErrorMessage = "Невірний формат email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введіть пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Підтвердіть пароль")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [DataType(DataType.Password)]
        [Display(Name = "Підтвердження паролю")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "Оберіть роль")]
        [Display(Name = "Роль")]
        public string Role { get; set; } = "Customer";
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Введіть ім'я")]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введіть email")]
        [EmailAddress(ErrorMessage = "Невірний формат email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Новий пароль (залиште порожнім щоб не міняти)")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Оберіть роль")]
        [Display(Name = "Роль")]
        public string Role { get; set; } = "Customer";
    }
}
