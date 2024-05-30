document.addEventListener('DOMContentLoaded', function () {
    const endDateInput = document.getElementById('end-date-input');
    const startDateInput = document.getElementById('start-date-input');
    const rentalId = document.getElementById('rental-id').value;
    const countOfDaysInput = document.getElementById('count-of-days-input');
    const totalPriceInput = document.getElementById('total-price-input');

    function calculateAndDisplayRentDetails() {
        const startDate = new Date(startDateInput.value);
        const endDate = new Date(endDateInput.value);

        if (startDate && endDate) {
            const durationInDays = Math.ceil((endDate - startDate) / (1000 * 60 * 60 * 24)) + 1;

            fetch(`/Account/CheckOut/${rentalId}?handler=RentDetails&duration=${durationInDays}`)
                .then(response => response.json())
                .then(data => {
                    if (data) {
                        document.getElementById('rental-days').textContent = durationInDays;
                        document.getElementById('price-coefficient').textContent = data.priceCoefficient;
                        document.getElementById('total-price').textContent = data.totalPrice;
                        document.getElementById('type-of-rent').value = data.typeOfRentId;

                        countOfDaysInput.value = durationInDays
                        totalPriceInput.value = data.totalPrice

                    }
                })
                .catch(error => console.error('Error fetching rent details:', error));
        }
    }

    endDateInput.addEventListener('change', calculateAndDisplayRentDetails);

    if (endDateInput.value) {
        calculateAndDisplayRentDetails();
    }
});