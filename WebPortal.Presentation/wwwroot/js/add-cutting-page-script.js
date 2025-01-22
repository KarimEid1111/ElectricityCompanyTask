document.addEventListener("DOMContentLoaded", () => {
    console.log("Script loaded and event listeners are being attached.");

    // Utility Functions
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
            : "";
    };

    const refreshTableData = async (selectedValue) => {
        const tableBody = document.querySelector("table tbody");
        const response = await fetch(`/CuttingDownSearch/GetCuttingsById?elementId=${selectedValue}`);
        const tableData = await response.json();

        // Clear existing rows
        tableBody.innerHTML = "";

        // Populate the table with updated data
        tableData.forEach((row) => {
            const tr = document.createElement("tr");
            tr.innerHTML = `
                <td class="border px-4 py-2">${row.networkElementName}</td>
                <td class="border px-4 py-2">${row.impactedCustomers}</td>
                <td class="border px-4 py-2">${row.cuttingDownDetailId}</td>
                <td class="border px-4 py-2">
                    <button type="button" class="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600" id="del-${row.cuttingDownDetailId}">Delete</button>
                </td>
            `;
            tableBody.appendChild(tr);
        });
    };

    // Event Handlers
    const attachArrowButtonEvent = (arrowButton) => {
        arrowButton.addEventListener("click", async (e) => {
            e.stopPropagation();
            const selectedCheckbox = document.querySelector(".form-checkbox:checked");
            if (selectedCheckbox) {
                const selectedValue = parseInt(selectedCheckbox.id.split("-")[1], 10);
                console.log("Selected value:", selectedValue);
                await refreshTableData(selectedValue);
            } else {
                alert("Please select a network element first.");
            }
        });
    };

    const attachDeleteEvent = async (e) => {
        if (e.target.tagName === "BUTTON" && e.target.id.startsWith("del-")) {
            e.stopPropagation();

            const selectedValue = parseInt(e.target.id.split("-")[1], 10);
            const confirmDelete = confirm(`Are you sure you want to delete the cutting record with ID: ${selectedValue}?`);

            if (confirmDelete) {
                try {
                    const response = await fetch(`/CuttingDownSearch/DeleteCutting?id=${selectedValue}`, {method: "DELETE"});
                    if (response.ok) {
                        alert("Cutting record deleted successfully!");
                        const selectedCheckbox = document.querySelector(".form-checkbox:checked");
                        const selectedValue = parseInt(selectedCheckbox.id.split("-")[1], 10);
                        await refreshTableData(selectedValue);
                    } else {
                        alert("Failed to delete the record. Please try again.");
                    }
                } catch (error) {
                    console.error("Error:", error);
                }
            }
        }
    };

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
                content.innerHTML = "";
                
                arrow.classList.toggle("rotate-90");
                content.classList.toggle("hidden");
            }


        });
    };

    const warnOnAdding = (event) => {
        const networkElementIdInput = document.getElementById("NetworkElementId1");
        if (!networkElementIdInput.value) {
            alert("Please select a network element");
            event.preventDefault();
        }
    }

    // Attach Initial Event Listeners
    const initialize = () => {
        const tableBody = document.querySelector("table tbody");
        const toggleButtons = document.querySelectorAll(".toggle-button");
        const checkboxes = document.querySelectorAll(".form-checkbox");
        const arrowButton = document.getElementById("arrowButton");
        const addButton = document.getElementById("addCuttingButton");

        checkboxes.forEach((checkbox) => checkbox.addEventListener("change", () => handleCheckboxSelection(checkbox)));
        toggleButtons.forEach((button) => attachToggleEvent(button));
        tableBody.addEventListener("click", attachDeleteEvent);
        addButton.addEventListener("click", warnOnAdding)
        attachArrowButtonEvent(arrowButton);
    };
    
    initialize();
});
