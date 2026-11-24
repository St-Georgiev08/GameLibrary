const form = document.getElementById("developerFilters");
const tbody = document.getElementById("devTable");
const search = document.getElementById("search");
const country = document.getElementById("country");
const foundedYear = document.getElementById("foundedYear");
const sortOrder = document.getElementById("sortOrder");

let activeRequest;

async function loadDevelopers() {
    activeRequest?.abort();
    activeRequest = new AbortController();
    const { signal } = activeRequest;

    const url = new URL(form.action);
    url.search = new URLSearchParams({
        search: search.value.trim(),
        country: country.value.trim(),
        foundedYear: foundedYear.value,
        sortOrder: sortOrder.value
    }).toString();

    try {
        const response = await fetch(url, { signal });

        if (!response.ok) {
            throw new Error(`Developer request failed (${response.status}).`);
        }

        const developers = await response.json();

        if (signal.aborted) {
            return;
        }

        tbody.replaceChildren();

        for (const developer of developers) {
            const row = tbody.insertRow();

            row.insertCell().textContent = developer.developerName ?? "";
            row.insertCell().textContent = developer.country ?? "";
            row.insertCell().textContent = developer.foundedYear ?? "";
            row.insertCell().textContent = `#${developer.id}`;
            row.insertCell().textContent = developer.email || "No information";

            const actions = document.createElement("div");
            actions.className = "table-actions";
            row.insertCell().append(actions);

            for (const action of ["Details", "Edit", "Delete"]) {
                const link = document.createElement("a");
                link.href = new URL(
                    `./${action}/${encodeURIComponent(developer.id)}`,
                    form.action
                ).href;
                link.className = `action-button ${action.toLowerCase()}-button`;
                link.textContent = action;
                actions.append(link);
            }
        }

        if (developers.length === 0) {
            const cell = tbody.insertRow().insertCell();
            cell.colSpan = 6;
            cell.className = "empty-table";
            cell.textContent = "No developers match the current filters.";
        }
    } catch (error) {
        if (error.name !== "AbortError") {
            console.error(error);
        }
    }
}

for (const input of [search, country, foundedYear]) {
    input.addEventListener("input", loadDevelopers);
}

sortOrder.addEventListener("change", loadDevelopers);

form.addEventListener("submit", (event) => {
    event.preventDefault();
    loadDevelopers();
});

document.getElementById("clearFilters").addEventListener("click", () => {
    form.reset();
    loadDevelopers();
});