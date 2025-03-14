// Saat fonksiyonu
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
updateClock(); // Sayfa açıldığında hemen çalıştır

// 'H' tuşuna basıldığında formları gizle/göster
document.addEventListener("keydown", function (event) {
    if (event.key.toLowerCase() === "h") {  
        let forms = document.querySelectorAll("form");
        forms.forEach(form => {
            form.style.display = (form.style.display === "none") ? "block" : "none";
        });
    }
});

// Login işlemi ve kullanıcı bilgilerini saklama
document.addEventListener('DOMContentLoaded', function () {
    const loginButton = document.querySelector('.login-btn'); // Login butonunu seç
    const usernameInput = document.getElementById('username'); // Kullanıcı adı input
    const passwordInput = document.getElementById('password'); // Şifre input

    // Kullanıcı bilgilerini saklamak için bir dizi
    let users = [];

    // Login butonuna tıklama
    loginButton.addEventListener('click', function (event) {
        event.preventDefault(); // Formun sayfayı yenilemesini engelle

        // Kullanıcı adı ve şifreyi
        const username = usernameInput.value.trim();
        const password = passwordInput.value.trim();

        // Kullanıcı bilgilerini diziye ekle
        users.push({ username, password });

        // Konsola diziyi yazdır
        console.log(users);

        // Kullanıcı bilgileri kontrolü
        if (username === "admin" && password === "admin") {
            alert("Login successful! Redirecting...");
            window.location.href = "table.html"; // table.html sayfasına yönlendir
        } else {
            alert("Invalid credentials. Try again.");
        }
    });
});

