async function loadDoctors() {
    // 1. Вземаме стойностите от полетата
    let search = document.getElementById("search").value;
    let country = document.getElementById("country").value;
    let foundedYear = document.getElementById("foundedYear").value;

    // 2. Подготвяме адреса на заявката
    let params = new URLSearchParams({
        search: search,
        country: country,
        foundedYear: foundedYear
    });

    // 3. Изпращаме заявката към контролера
    let response = await fetch(
        "/Developer/GetAllFilteredAsync?" + params
    );

    // 4. Преобразуваме JSON отговора в JavaScript обекти
    let devs = await response.json();

    // 5. Изчистваме старите редове от таблицата
    let tbody = document.getElementById("devTable");
    tbody.innerHTML = "";

    // 6. Добавяме ред за всеки намерен лекар
    devs.forEach(function (dev) {
        let row = document.createElement("tr");

        [dev.firstName, dev.lastName,
        dev.specialty, dev.hospitalName].forEach(function (value) {
            let cell = document.createElement("td");
            cell.textContent = value ?? "";
            row.appendChild(cell);
        });

        // Колона с линкове за действия
        let actions = document.createElement("td");

        ["Edit", "Details", "Delete"].forEach(function (action, index) {
            if (index > 0) {
                actions.appendChild(document.createTextNode(" | "));
            }

            let link = document.createElement("a");
            link.href = "/Doctor/" + action + "/" + doctor.id;
            link.textContent = action;
            actions.appendChild(link);
        });

        row.appendChild(actions);
        tbody.appendChild(row);
    });
}

// Автоматично филтриране
document.getElementById("search")
    .addEventListener("input", loadDoctors);

document.getElementById("specialty")
    .addEventListener("input", loadDoctors);

document.getElementById("sortOrder")
    .addEventListener("change", loadDoctors);

// Изчистване на филтрите
document.getElementById("clearFilters")
    .addEventListener("click", function () {
        document.getElementById("search").value = "";
        document.getElementById("specialty").value = "";
        document.getElementById("sortOrder").value = "name_asc";

        loadDoctors();
    }); 