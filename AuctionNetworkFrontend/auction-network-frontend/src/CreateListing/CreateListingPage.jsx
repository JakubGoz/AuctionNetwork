import React, { useState, useEffect } from 'react';
import { baseUrl, authorization } from '../Shared/Options/ApiOptions';
import axios from 'axios';
import classes from './CreateListingPage.module.scss';
import Select from 'react-select';

const CreateListingPage = () => {
    const [categories, setCategories] = useState([]);
    const [listing, setListing] = useState({
        title: '',
        description: '',
        price: 0,
        buyNowPrice: 0,
        endDate: '',
        categoryId: 0,
        isAuction: false
    });

    // Fetch categories from backend
    useEffect(() => {
        const fetchCategories = async () => {
            try {
                const response = await axios.get(`${baseUrl}/category`, authorization(localStorage.getItem("token")));
                console.log('API response:', response);
                if (Array.isArray(response.data)) {
                    const options = response.data.map((category) => ({
                        value: category.id,
                        label: category.name,
                    }));
                    setCategories(options);
                } else {
                    console.error('Response is not an array:', response.data);
                }
            } catch (err) {
                console.error('Error fetching categories:', err);
            }
        };

        fetchCategories();
    }, []);

    // Handle changes in the form fields
    const handleInputChange = (e) => {
        const { name, value, type, checked } = e.target;
        setListing((prev) => ({
            ...prev,
            [name]: type === 'checkbox' ? checked : value
        }));
    };

    // Handle category change
    const handleCategoryChange = (selectedOption) => {
        setListing((prev) => ({
            ...prev,
            categoryId: selectedOption.value
        }));
    };

    // Handle form submission (create listing)
    const handleSubmit = async (e) => {
        e.preventDefault();
        const token = localStorage.getItem("token");

        if (!token) {
            alert("You must be logged in to create a listing.");
            return;
        }

        // Validate if endDate is in the future
        if (listing.endDate && new Date(listing.endDate) <= new Date()) {
            alert('End Date must be in the future!');
            return;
        }

        try {
            const headers = {
                Authorization: `Bearer ${token}`,
                "Content-Type": "application/json" // Ensure the content type is JSON
            };

            // Format endDate to the correct format if provided
            let formattedEndDate = null;
            if (listing.endDate) {
                formattedEndDate = new Date(listing.endDate).toISOString(); // Convert to ISO string
            }

            // Construct the payload
            const payload = {
                title: listing.title,
                description: listing.description,
                price: listing.price,
                categoryId: listing.categoryId,
                isAuction: listing.isAuction,
                buyNowPrice: listing.buyNowPrice || null, // Optional field
                endDate: formattedEndDate // Correctly formatted date or null
            };

            // Send the request as JSON
            await axios.post(`${baseUrl}/listing`, JSON.stringify(payload), { headers });

            alert('Listing created successfully!');
        } catch (err) {
            console.error('Error submitting listing:', err);
            alert('Failed to create listing');
        }
    };

    // Get current date and time in ISO format for the min attribute
    const currentDateTime = new Date().toISOString().slice(0, 16); // Format it as yyyy-MM-ddTHH:mm

    return (
        <div className={classes.container}>
            <h1>Create Listing</h1>
            <form onSubmit={handleSubmit}>
                <div className={classes.formGroup}>
                    <label>Title</label>
                    <input
                        type="text"
                        name="title"
                        value={listing.title}
                        onChange={handleInputChange}
                        required
                    />
                </div>

                <div className={classes.formGroup}>
                    <label>Description</label>
                    <textarea
                        name="description"
                        value={listing.description}
                        onChange={handleInputChange}
                        required
                    />
                </div>

                <div className={classes.formGroup}>
                    <label>Price</label>
                    <input
                        type="number"
                        name="price"
                        value={listing.price}
                        onChange={handleInputChange}
                        required
                    />
                </div>

                <div className={classes.formGroup}>
                    <label>Category</label>
                    <Select
                        options={categories}
                        onChange={handleCategoryChange}
                        value={categories.find(cat => cat.value === listing.categoryId)}
                        placeholder="Select a category"
                    />
                </div>

                <div className={classes.formGroup}>
                    <label>On Auction</label>
                    <input
                        type="checkbox"
                        name="isAuction"
                        checked={listing.isAuction}
                        onChange={handleInputChange}
                    />
                </div>

                {listing.isAuction && (
                    <>
                        <div className={classes.formGroup}>
                            <label>Buy Now Price (optional)</label>
                            <input
                                type="number"
                                name="buyNowPrice"
                                value={listing.buyNowPrice}
                                onChange={handleInputChange}
                            />
                        </div>

                        <div className={classes.formGroup}>
                            <label>End Date</label>
                            <input
                                type="datetime-local"
                                name="endDate"
                                value={listing.endDate}
                                onChange={handleInputChange}
                                min={currentDateTime} // Prevent selecting past dates and times
                            />
                        </div>
                    </>
                )}

                <button type="submit" className={classes.submitButton}>
                    Create Listing
                </button>
            </form>
        </div>
    );
};

export default CreateListingPage;
