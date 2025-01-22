document.addEventListener('DOMContentLoaded', () => {
    // Element References
    const sourceCurrencyDropdown = document.getElementById('source-currency-name');
    const targetCurrencyDropdown = document.getElementById('target-currency-name');
    const sourceCurrencyAmountInput = document.getElementById('source-currency-amount');
    const targetCurrencyAmountInput = document.getElementById('target-currency-amount');
    const dateInput = document.querySelector('.date-picker');
    const sourceToTargetRateLabel = document.getElementById('source-to-target-rate');
    const targetToSourceRateLabel = document.getElementById('target-to-source-rate');

    // Source of truth rates: Parsed from UI
    let sourceToTargetRate = parseFloat(document.getElementById('source-to-target-rate').textContent.split(': ')[1]) || 1.00;
    let targetToSourceRate = parseFloat(document.getElementById('target-to-source-rate').textContent.split(': ')[1]) || 1.00;

    // Fetch conversion rates when the source currency changes
    sourceCurrencyDropdown.addEventListener('change', async () => {
        await fetchConversionRates('source');
    });

    // Fetch conversion rates when the target currency changes
    targetCurrencyDropdown.addEventListener('change', async () => {
        await fetchConversionRates('target');
    });

    // Fetch conversion rates when the date changes
    dateInput.addEventListener('change', async () => {
        await fetchConversionRates('date');
    });

    // Recalculate target amount when source amount is changed
    sourceCurrencyAmountInput.addEventListener('input', () => {
        const sourceAmount = parseFloat(sourceCurrencyAmountInput.value) || 0;
        targetCurrencyAmountInput.value = (sourceAmount * sourceToTargetRate).toFixed(2);
    });

    // Recalculate source amount when target amount is changed
    targetCurrencyAmountInput.addEventListener('input', () => {
        const targetAmount = parseFloat(targetCurrencyAmountInput.value) || 0;
        sourceCurrencyAmountInput.value = (targetAmount * targetToSourceRate).toFixed(2);
    });

    // Function to fetch conversion rates
    async function fetchConversionRates(triggeredBy) {
        const sourceCurrencyName = sourceCurrencyDropdown.value.toLowerCase();
        const targetCurrencyName = targetCurrencyDropdown.value.toLowerCase();
        const date = dateInput.value;
        const rateStatus = document.getElementById('rate-status');

        // API Call only happens when currencies and date are populated
        if (!sourceCurrencyName || !targetCurrencyName || !date) {
            return;
        }

        // Validate date isn't in future
        const selectedDate = new Date(date);
        const today = new Date();

        if(selectedDate > today)
        {
            updateStatusMessage(rateStatus, "Date cannot be in the future.", 'error');
            return;
        }

        // Show "Fetching rates..." message
        updateStatusMessage(rateStatus, "Retrieving exchange rates... This may take a moment if it's the first request.", 'fetching');

        try {
            const response = await fetch('/api/CurrencyExchangeApi/GetConversionRates', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    sourceCurrencyName: sourceCurrencyName,
                    targetCurrencyName: targetCurrencyName,
                    date: date
                })
            });

            if (response.ok) {
                const data = await response.json();
                sourceToTargetRate = data.sourceToTargetRate || 1.00;
                targetToSourceRate = data.targetToSourceRate || 1.00;

                // Update Input Fields
                if (triggeredBy === 'source') {
                    const targetAmount = parseFloat(targetCurrencyAmountInput.value) || 0;
                    sourceCurrencyAmountInput.value = (targetAmount * targetToSourceRate).toFixed(2);
                }

                if (triggeredBy === 'target' || triggeredBy === 'date') {
                    const sourceAmount = parseFloat(sourceCurrencyAmountInput.value) || 0;
                    targetCurrencyAmountInput.value = (sourceAmount * sourceToTargetRate).toFixed(2);
                }

                // Update Conversion Display
                sourceToTargetRateLabel.textContent = `${sourceCurrencyDropdown.value} To ${targetCurrencyDropdown.value} Rate: ${sourceToTargetRate}`;
                targetToSourceRateLabel.textContent = `${targetCurrencyDropdown.value} to ${sourceCurrencyDropdown.value} Rate: ${targetToSourceRate}`;

                // Clear the status label
                updateStatusMessage(rateStatus, "", '');
            } else {
                updateStatusMessage(rateStatus, "Failed to fetch rates", 'error');
            }
        } catch (error) {
            updateStatusMessage(rateStatus, "Error fetching rates", 'error');
        }
    }
});

// Utility Functions
function updateStatusMessage(statusElement, message, statusClass) {
    statusElement.textContent = message;
    statusElement.classList.remove('fetching', 'error');
    if (statusClass) {
        statusElement.classList.add(statusClass);
    }
}
