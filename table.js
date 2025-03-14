// Çıkış yap fonksiyonu
function logout() {
    alert("Logging out...");
    window.location.href = "index.html";
}

// Formu ve tabloyu seç
const playerForm = document.getElementById('player-form');
const tableBody = document.getElementById('table-body');
let playerCount = 0;

// Form gönderildiğinde çalıştır
playerForm.addEventListener('submit', function (event) {
    event.preventDefault();

    // Formdan verileri al
    const playerName = document.getElementById('player-name').value.trim();
    const club = document.getElementById('club').value.trim();
    const position = document.getElementById('position').value.trim();
    const goals = document.getElementById('goals').value.trim();

    if (playerName === "" || club === "" || position === "" || goals === "") {
        alert("Please fill in all fields!");
        return;
    }

    // Yeni tablo satırı oluştur
    playerCount++;
    const newRow = document.createElement('tr');
    newRow.innerHTML = `
        <td>${playerCount}</td>
        <td>${playerName}</td>
        <td>${club}</td>
        <td>${position}</td>
        <td>${goals}</td>
        <td><button class="delete-btn">❌</button></td>
    `;

    // Silme butonu ekleyerek tabloya satırı ekle
    tableBody.appendChild(newRow);

    // Silme butonuna olay ekleyelim
    newRow.querySelector('.delete-btn').addEventListener('click', function () {
        newRow.remove();
        playerCount--;
        updateRowNumbers();
    });

    // Formu temizle
    playerForm.reset();
});

// Satır numaralarını güncelleme fonksiyonu
function updateRowNumbers() {
    let rows = tableBody.querySelectorAll('tr');
    rows.forEach((row, index) => {
        row.cells[0].textContent = index + 1;
    });
}
