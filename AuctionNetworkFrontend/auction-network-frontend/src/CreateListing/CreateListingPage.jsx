import React, { useState, useEffect } from 'react';
import { baseUrl, authorization } from '../Shared/Options/ApiOptions';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';
import classes from './CreateListingPage.module.scss';
import Select from 'react-select';

const CreateListingPage = () => {
    const { listingId } = useParams(); 
    const navigate = useNavigate();
    const [isEditMode, setIsEditMode] = useState(false);
    const [categories, setCategories] = useState([]);
    const [listing, setListing] = useState({
        title: '',
        description: '',
        price: 0,
        buyNowPrice: 0,
        endDate: '',
        categoryId: 0,
        isAuction: false,
        listingPicture: null,
    });

    useEffect(() => {
        const fetchCategories = async () => {
            try {
                const response = await axios.get(`${baseUrl}/category`, authorization(localStorage.getItem("token")));
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

        const fetchListing = async () => {
            if (listingId) {
                setIsEditMode(true);
                try {
                    const response = await axios.get(`${baseUrl}/listing/${listingId}`, authorization(localStorage.getItem("token")));
                    setListing({
                        title: response.data.title,
                        description: response.data.description,
                        price: response.data.price,
                        buyNowPrice: response.data.buyNowPrice || 0,
                        endDate: response.data.endDate ? new Date(response.data.endDate).toISOString().slice(0, 16) : '',
                        categoryId: response.data.categoryId,
                        isAuction: response.data.isAuction,
                        listingPicture: null, // Nie pobieramy zdjęcia tutaj
                    });
                } catch (err) {
                    console.error('Error fetching listing details:', err);
                }
            }
        };

        fetchCategories();
        fetchListing();
    }, [listingId]);

    const handleInputChange = (e) => {
        const { name, value, type, checked } = e.target;
        setListing((prev) => ({
            ...prev,
            [name]: type === 'checkbox' ? checked : value,
        }));
    };

    const handleCategoryChange = (selectedOption) => {
        setListing((prev) => ({
            ...prev,
            categoryId: selectedOption.value,
        }));
    };

    const handleImageChange = (e) => {
        const file = e.target.files[0];
        setListing((prev) => ({
            ...prev,
            listingPicture: file,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const token = localStorage.getItem("token");
        if (!token) {
            alert("You must be logged in.");
            return;
        }
    
        if (listing.endDate && new Date(listing.endDate) <= new Date()) {
            alert('End Date must be in the future!');
            return;
        }
    
        try {
            
            const formData = new FormData();
            formData.append('title', listing.title);
            formData.append('description', listing.description);
            formData.append('price', listing.price);
            formData.append('categoryId', listing.categoryId);
            formData.append('isAuction', listing.isAuction);
            if (listing.buyNowPrice) formData.append('buyNowPrice', listing.buyNowPrice);
            if (listing.endDate) formData.append('endDate', new Date(listing.endDate).toISOString());
            if (listing.listingPicture) formData.append('listingPicture', listing.listingPicture);
            for (let [key, value] of formData.entries()) {
                console.log(`${key}: ${value}`);
            }
            console.log("Listing ID:", listingId);

    
            const headers = { Authorization: `Bearer ${token}`, "Content-Type": "multipart/form-data" };
    
            if (listingId) {
                await axios.put(`${baseUrl}/listing/update-listing/${listingId}`, formData, { headers });
                alert('Listing updated successfully!');
            } else {
                await axios.post(`${baseUrl}/listing`, formData, { headers });
                alert('Listing created successfully!');
            }
        } catch (err) {
            console.error('Error submitting listing:', err);
            alert('Failed to submit listing');
        }
    };
    
    return (
        <div className={classes.container}>
            <h1>{isEditMode ? 'Update Listing' : 'Create Listing'}</h1>
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
                        value={categories.find((cat) => cat.value === listing.categoryId)}
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
                                min={new Date().toISOString().slice(0, 16)}
                            />
                        </div>
                    </>
                )}
                <div className={classes.formGroup}>
                    <label>Photo</label>
                    <input
                        type="file"
                        name="listingPicture"
                        accept="listingPicture/*"
                        onChange={handleImageChange}
                    />
                </div>

                <button className={classes.submitButton} type="submit">{isEditMode ? 'Update Listing' : 'Create Listing'}</button>
            </form>
        </div>
    );
};

export default CreateListingPage;
