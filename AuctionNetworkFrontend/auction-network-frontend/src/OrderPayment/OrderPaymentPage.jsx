import React, { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate, useParams } from "react-router-dom";
import { baseUrl } from "../Shared/Options/ApiOptions";
import { Elements, CardElement, useStripe, useElements } from "@stripe/react-stripe-js";
import { loadStripe } from "@stripe/stripe-js";
import classes from "./OrderPaymentPage.module.scss"; 
import noImageAvailable from "../Shared/No_Image_Available.jpg"; 

const stripePromise = loadStripe("pk_test_51QpSJfRosBdICgRypdOZ1SaV1syvhoyIejG9X2wrBFmGI1qEg5lhYjLdoXi5mcJo0ri2gSBAigI70DhWDFBzxgLn003pi87osf"); // 🔹 Podmień na prawdziwy klucz Stripe

const CheckoutForm = ({ clientSecret, orderPrice, paymentResponseData }) => {
  const stripe = useStripe();
  const elements = useElements();
  const navigate = useNavigate();
  const [isProcessing, setIsProcessing] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!stripe || !elements) return;

    setIsProcessing(true);

    const cardElement = elements.getElement(CardElement);
    const { error, paymentIntent } = await stripe.confirmCardPayment(clientSecret, {
      payment_method: { card: cardElement },
    });

    if (error) {
      console.error("Payment error:", error);
      alert("Payment failed. Try again.");
      setIsProcessing(false);
      return;
    }

    try {
      await axios.post(
        `${baseUrl}/payment/finalize`,
        { 
          paymentId: paymentResponseData, // Użyj paymentResponseData
          IsSuccessful: paymentIntent.status === "succeeded"
        },
        { headers: { Authorization: `Bearer ${localStorage.getItem("token")}` } }
      );

      alert("Payment successful!");
      navigate("/order-success");
    } catch (err) {
      console.error("Error finalizing payment:", err);
      alert("Payment confirmation failed.");
    } finally {
      setIsProcessing(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className={classes["payment-form"]}>
      <h2>Total: {orderPrice} PLN</h2>
      <CardElement className={classes["card-input"]} />
      <button type="submit" disabled={isProcessing || !stripe} className={classes["submit-button"]}>
        {isProcessing ? "Processing..." : "Pay Now"}
      </button>
    </form>
  );
};

const OrderPaymentPage = () => {
  const [orderData, setOrderData] = useState({
    firstName: "",
    lastName: "",
    country: "",
    city: "",
    street: "",
    postalCode: "",
    phoneNumber: "",
  });

  const [isSubmitting, setIsSubmitting] = useState(false);
  const { listingId } = useParams();
  const [listing, setListing] = useState(null);
  const [photoUrl, setPhotoUrl] = useState(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();
  const [showPayment, setShowPayment] = useState(false);
  const [clientSecret, setClientSecret] = useState(null);
  const [orderPrice, setOrderPrice] = useState(null);
  const [paymentResponseData, setPaymentResponseData] = useState(null);

  useEffect(() => {
    const fetchListing = async () => {
      try {
        const token = localStorage.getItem("token");
        const listingRes = await axios.get(`${baseUrl}/listing/${listingId}`, {
          headers: { Authorization: `Bearer ${token}` },
        });

        setListing(listingRes.data);

        const photoResponse = await fetch(`${baseUrl}/listing/${listingId}/listing-picture`, {
          method: "GET",
          headers: { Authorization: `Bearer ${token}` },
        });

        if (photoResponse.ok) {
          const blob = await photoResponse.blob();
          setPhotoUrl(URL.createObjectURL(blob));
        } else {
          console.error("Failed to fetch the listing picture.");
        }
      } catch (error) {
        console.error("Error fetching listing:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchListing();
  }, [listingId]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setOrderData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const token = localStorage.getItem("token");
  
    if (!token) {
      alert("You must be logged in to create an order.");
      return;
    }
  
    setIsSubmitting(true);
  
    try {
      const orderResponse = await axios.post(
        `${baseUrl}/order/create`,
        { listingId, ...orderData },
        { headers: { Authorization: `Bearer ${token}` } }
      );
  
      setOrderPrice(listing?.price); 
  
      const paymentResponse = await axios.post(
        `${baseUrl}/payment/payment`,
        { orderId: orderResponse.data },
        { headers: { Authorization: `Bearer ${token}` } }
      );
  
      setPaymentResponseData(paymentResponse.data); 
  
      const intentResponse = await axios.get(
        `${baseUrl}/payment/intent/${paymentResponse.data}`,
        { headers: { Authorization: `Bearer ${token}` } }
      );
  
      setClientSecret(intentResponse.data.clientSecret);
      setShowPayment(true);
    } catch (err) {
      console.error("Error creating order:", err);
      alert("Failed to create order");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className={classes["main-container"]}>
      <div className={classes["container"]}>
        <div className={classes["form-container"]}>
          <h1>Create Order</h1>
          <form onSubmit={handleSubmit}>
            <div className={classes.formGroup}>
              <label>First Name</label>
              <input
                type="text"
                name="firstName"
                value={orderData.firstName}
                onChange={handleInputChange}
                required
              />
            </div>
  
            <div className={classes.formGroup}>
              <label>Last Name</label>
              <input
                type="text"
                name="lastName"
                value={orderData.lastName}
                onChange={handleInputChange}
                required
              />
            </div>
  
            <div className={classes.formGroup}>
              <label>Phone Number</label>
              <input
                type="text"
                name="phoneNumber"
                value={orderData.phoneNumber}
                onChange={handleInputChange}
                required
              />
            </div>
  
            <div className={classes.formGroup}>
              <label>Country</label>
              <input
                type="text"
                name="country"
                value={orderData.country}
                onChange={handleInputChange}
                required
              />
            </div>
  
            <div className={classes.formGroup}>
              <label>Postal Code</label>
              <input
                type="text"
                name="postalCode"
                value={orderData.postalCode}
                onChange={handleInputChange}
                required
              />
            </div>
  
            <div className={classes.formGroup}>
              <label>City</label>
              <input
                type="text"
                name="city"
                value={orderData.city}
                onChange={handleInputChange}
                required
              />
            </div>
  
            <div className={classes.formGroup}>
              <label>Street</label>
              <input
                type="text"
                name="street"
                value={orderData.street}
                onChange={handleInputChange}
                required
              />
            </div>
  
            <div className={classes.formGroup}>
              <button type="submit" className={classes.submitButton} disabled={isSubmitting}>
                {isSubmitting ? 'Submitting...' : 'Create Order'}
              </button>
            </div>
          </form>
        </div>
  
        <div className={classes["listing-container"]}>
          <strong>{listing?.title}</strong>
          <p><strong>Seller:</strong> {listing?.sellerUserName}</p>
          <p><strong>Price:</strong> {listing?.price} PLN</p>
          <div className={classes["image-container"]}>
              <img src={photoUrl || noImageAvailable} alt="Listing" className={classes.image} />
          </div>
        </div>
      </div>
  
      {showPayment && clientSecret && (
  <div className={classes["payment-container"]}>
    <h2>Payment</h2>
    <Elements stripe={stripePromise} options={{ clientSecret }}>
      <CheckoutForm
        clientSecret={clientSecret}
        orderPrice={orderPrice}
        paymentResponseData={paymentResponseData} 
      />
    </Elements>
  </div>
)}
    </div>
  );
}

export default OrderPaymentPage;