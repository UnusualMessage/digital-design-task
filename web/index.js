const select = document.querySelector(".header-title .numbers-api-select");
const form = document.querySelector(".header-title .numbers-api");
const text = document.querySelector(".header-title .numbers-api-text");
const title = document.querySelector(".header-title .numbers-api-number");

let currentType = "date";

const updateText = async () => {
    const response = await getFact(currentType);

    if (response.date) {
        title.innerHTML = `${response.date} ${response.number}`
    } else {
        title.innerHTML = `${response.number}`
    }

    text.innerHTML = response.text;
}

const getFact = async (type) => {
    const response = await fetch(`http://numbersapi.com/random/${type}?json`);
    return await response.json();
}

const onSubmit = async (event) => {
    event.preventDefault();
    await updateText(currentType);
}

const onSelect = (event) => {
    currentType = event.target.value;
}

document.addEventListener("DOMContentLoaded", async () => {
    await updateText(currentType);

    setInterval(async () => {
        await updateText(currentType);
    }, 5000)
})

form.addEventListener("submit", onSubmit);
select.addEventListener("change", onSelect);
