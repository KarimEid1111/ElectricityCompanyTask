const attachToggleEvent = (button) => {
    button.addEventListener("click", async (e) => {
        e.stopPropagation();
        const buttonId = button.dataset.id;
        const content = document.getElementById(`children-${buttonId}`);
        const arrow = button.querySelector(".arrow");

        if (content && content.classList.contains("hidden")) {
            if (!content.hasChildNodes()) {
                const response = await fetch(`/CuttingDownSearch/GetChildren?parentId=${buttonId}`);
                const children = await response.json();

                if (children.length > 0) {
                    children.forEach((child) => {
                        const li = document.createElement("li");
                        li.innerHTML = `
                                <div class="flex items-center space-x-2">
                                    <input type="checkbox" class="form-checkbox" id="box-${child.id}" value="${child.networkElementTypeId}" name="selectedElements">
                                    <button type="button" class="flex items-center space-x-2 toggle-button" id="button-${child.id}" data-id="${child.id}">
                                        <span class="arrow">&rarr;</span>
                                        <span>${child.networkElementName}</span>
                                    </button>
                                </div>
                                <ul class="pl-6 space-y-2 toggle-content hidden" id="children-${child.id}"></ul>
                            `;
                        content.appendChild(li);

                        // Attach new event listeners
                        const newButton = document.getElementById(`button-${child.id}`);
                        attachToggleEvent(newButton);
                        const newCheckbox = document.getElementById(`box-${child.id}`);
                        newCheckbox.addEventListener("change", () => handleCheckboxSelection(newCheckbox));
                    });
                } else {
                    content.innerHTML = "";
                }
            }
            content.classList.toggle("hidden");
            if (arrow) {
                arrow.classList.toggle("rotate-90");
            }
        } else if (content) {
            arrow.classList.toggle("rotate-90");
            content.classList.toggle("hidden");
        }


    });
};

const handleCheckboxSelection = (checkbox) => {
    const checkboxes = document.querySelectorAll(".form-checkbox");
    checkboxes.forEach((cb) => {
        if (cb !== checkbox) {
            cb.checked = false;
        }
    });
    const hiddenInput = document.getElementById("NetworkElementId1");
    hiddenInput.value = checkbox.checked
        ? parseInt(checkbox.id.split("-")[1], 10)
        : ""; // Clear the value if no checkbox is selected
};
const buildHierarchy = (parent, container, searchKey) => {
    parent.children.forEach((child) => {
        const li = document.createElement("li");
        li.innerHTML = `
                <div class="flex items-center space-x-2">
                    <input type="checkbox" class="form-checkbox" id="box-${child.id}" value="${child.networkElementTypeId}" name="selectedElements">
                    <button type="button" class="flex items-center space-x-2 toggle-button" id="button-${child.id}" data-id="${child.id}">
                        <span class="arrow">&rarr;</span>
                        <span>${child.networkElementName}</span>
                    </button>
                </div>
                <ul class="pl-6 space-y-2 toggle-content hidden" id="children-${child.id}"></ul>
            `;
        container.appendChild(li);

        const newButton = document.getElementById(`button-${child.id}`);
        attachToggleEvent(newButton);

        const newCheckbox = document.getElementById(`box-${child.id}`);
        newCheckbox.addEventListener("change", () => handleCheckboxSelection(newCheckbox));
        if(searchKey.toLowerCase() === child.networkElementName.toLowerCase()){
            const checkboxes = document.querySelectorAll(".form-checkbox");
            checkboxes.forEach((cb) => {
                cb.checked = false;
            });
            newCheckbox.checked = true;
        }

        if (child.children && child.children.length > 0) {
            const childContainer = document.getElementById(`children-${child.id}`);
            buildHierarchy(child, childContainer, searchKey);
        }

    });
    if (parent.networkElementName !== searchKey) {
        const arrow = container.previousElementSibling?.querySelector(".arrow");
        if (arrow) {
            arrow.classList.toggle("rotate-90");
        }
    }
    container.classList.toggle("hidden");
};
const expandHierarchy = async () => {

    const inputValue = document.querySelector("input[placeholder='Search Value']").value.trim();
    const response = await fetch(`/CuttingDownSearch/GetHierarchyPath?searchValue=${encodeURIComponent(inputValue)}`);
    const hierarchy = await response.json();
    const content = document.getElementById(`children-${hierarchy.id}`);

    content.innerHTML = "";

    buildHierarchy(hierarchy, content, inputValue);
};

const triggerHierarchyExpansion = (button) => {
    button.addEventListener("click", () => {
        expandHierarchy();
    });
};
document.addEventListener("DOMContentLoaded", () => {
    // Select the search button and input field
    const searchButton = document.querySelector("#searchNetwork"); // Your search button class
    if (searchButton) {
        // Attach the hierarchy expansion logic
        triggerHierarchyExpansion(searchButton);
    } else {
        console.error("Search button not found. Please ensure the button exists in your HTML.");
    }
});
