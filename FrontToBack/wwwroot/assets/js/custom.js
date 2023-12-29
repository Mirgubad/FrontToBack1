﻿document.addEventListener("DOMContentLoaded", () => {
    let size = 1;
    let page =1;
    const moreButton = document.getElementById("more");

    moreButton.addEventListener("click", async () => {
        try {
            const response = await fetch(`/pricing/Load?size=${size}&page=${page}`);
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }

            const result = await response.text();
            document.getElementById("data").insertAdjacentHTML("afterend", result);
           
            size++;
            if (size==3) {
                moreButton.style.opacity = "0";
            } else {

                moreButton.style.opacity = "1";
            }
        } catch (error) {
            console.error("Error fetching data:", error);
        }
    });
});