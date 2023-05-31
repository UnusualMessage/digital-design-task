const select = document.querySelector(".header-title .numbers-api-select");
const form = document.querySelector(".header-title .numbers-api");
const text = document.querySelector(".header-title .numbers-api-text");

let currentType = "date";

const updateText = async (type) => {
    text.innerHTML = await getFact(currentType);
}

const getFact = async (type) => {
    const response = await fetch(`http://numbersapi.com/random/${type}`);
    return await response.text();
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
