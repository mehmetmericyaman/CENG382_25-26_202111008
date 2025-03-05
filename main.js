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

