function updateClock() {
    let now = new Date();
    let hours = now.getHours().toString().padStart(2, '0');
    let minutes = now.getMinutes().toString().padStart(2, '0');
    let seconds = now.getSeconds().toString().padStart(2, '0');
    let timeString = `${hours}:${minutes}:${seconds}`;
    
    document.getElementById('clock').textContent = timeString;
}

// Her saniye saati güncelle
setInterval(updateClock, 1000);

// Sayfa açıldığında hemen başlat
updateClock();

document.addEventListener("keydown", function (event) {
    if (event.key.toLowerCase() === "h") {  // 'H' veya 'h' tuşuna basıldığında
        let forms = document.querySelectorAll("form");
        forms.forEach(form => {
            if (form.style.display === "none") {
                form.style.display = "block";  // Formları göster
            } else {
                form.style.display = "none";  // Formları gizle
            }
        });
    }
});
document.addEventListener('DOMContentLoaded', function () {
    const loginButton = document.querySelector('.login-btn'); // Login butonunu seç
    const usernameInput = document.getElementById('username'); // Kullanıcı adı input
    const passwordInput = document.getElementById('password'); // Şifre input

    // Kullanıcı bilgilerini saklamak için bir dizi
    let users = [];

    // Login butonuna tıklama olayını ekle
    loginButton.addEventListener('click', function (event) {
        event.preventDefault(); // Formun sayfayı yenilemesini engelle

        // Kullanıcı adı ve şifreyi al
        const username = usernameInput.value;
        const password = passwordInput.value;

        // Dizinin içine kullanıcı adı ve şifreyi ekle
        users.push({ username, password });

        // Konsola diziyi yazdır
        console.log(users);
    });
});
