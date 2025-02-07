import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faStar } from "@fortawesome/free-solid-svg-icons";
import axios from "axios";
import classes from "./ListingComponent.module.scss";
import ReviewModal from "./ReviewModal";
import { baseUrl } from "../../Shared/Options/ApiOptions";
import noImageAvailable from "../../Shared/No_Image_Available.jpg";

const ListingComponent = ({ listing }) => {
  const isAuction = listing.isAuction;
  const navigate = useNavigate();
  const [showModal, setShowModal] = useState(false);
  const [photoUrl, setPhotoUrl] = useState(null);
  const [loading, setLoading] = useState(true);
  const [userInfo, setUserInfo] = useState(null);

  // Pobieranie informacji o użytkowniku
  useEffect(() => {
    const fetchUserInfo = async () => {
      try {
        const token = localStorage.getItem("token");
        const response = await axios.get(`${baseUrl}/user/user-short-info`, {
          headers: { Authorization: `Bearer ${token}` },
        });

        if (response.status === 200) {
          setUserInfo(response.data);
        }
      } catch (error) {
        console.error("Error fetching user info:", error);
      }
    };

    fetchUserInfo();
  }, []);

  // Pobieranie zdjęcia
  useEffect(() => {
    const fetchPhoto = async () => {
      try {
        const token = localStorage.getItem("token");
        const response = await fetch(
          `${baseUrl}/listing/${listing.listingId}/listing-picture`,
          {
            method: "GET",
            headers: { Authorization: `Bearer ${token}` },
          }
        );

        if (response.ok) {
          const blob = await response.blob();
          const url = URL.createObjectURL(blob);
          setPhotoUrl(url);
        }
      } catch (error) {
        console.error("Error fetching listing picture:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchPhoto();
  }, [listing.listingId]);

  // Funkcja usuwania listingu
  const handleDelete = async () => {
    if (window.confirm("Are you sure you want to delete this listing?")) {
      try {
        const token = localStorage.getItem("token");
        await axios.delete(`${baseUrl}/listing/${listing.listingId}`, {
          headers: { Authorization: `Bearer ${token}` },
        });
        alert("Listing deleted successfully!");
        window.location.reload(); // Przekierowanie
      } catch (error) {
        console.error("Error deleting listing:", error);
        alert("An error occurred while deleting the listing.");
      }
    }
  };

  // Funkcja przejścia do składania zamówienia
  const handleBuyNow = () => {
    navigate(`/order-payment/${listing.listingId}`);
  };

  const isSeller = userInfo && listing.sellerUserName === userInfo.userName;
  const isWinner =
    userInfo &&
    listing.listingStatus === "Ended" &&
    listing.winnerId &&
    listing.winnerId === userInfo.userId;

  return (
    <div className={classes["listing-item"]}>
      <div className={classes["image-container"]}>
        {loading ? (
          <p>Loading image...</p>
        ) : photoUrl ? (
          <img
            src={photoUrl}
            alt={`Listing ${listing.title}`}
            className={classes["listing-image"]}
          />
        ) : (
          <img
            src={noImageAvailable}
            alt="No image available"
            className={classes["listing-image"]}
          />
        )}
      </div>

      <div className={classes["left-container"]}>
        <div className={classes["details-container"]}>
          <div>
            <strong>Title:</strong> {listing.title}
          </div>
          <div>
            <strong>Status:</strong> {listing.listingStatus}
          </div>
          <div>
            <strong>Start Date:</strong>{" "}
            {new Date(listing.startDate).toLocaleDateString()}
          </div>
          {listing.endDate && (
            <div>
              <strong>End Date:</strong>{" "}
              {new Date(listing.endDate).toLocaleString()}
            </div>
          )}
          <div>
            <strong>Reviews Count:</strong> {listing.listingReviewsCount}
          </div>
          <div>
            <strong>Average Rating:</strong> {listing.listingReviewsAvg}
          </div>
        </div>
      </div>

      <div className={classes["right-container"]}>
  <div>
    <strong>Seller:</strong> {listing.sellerUserName}
  </div>
  <div>
    <strong>Price:</strong> {listing.price} PLN
  </div>
  {listing.buyNowPrice && (
    <div>
      <strong>Buy Now Price:</strong> {listing.buyNowPrice} PLN
    </div>
  )}

        
        {/* Przyciski akcji */}
        <button className={classes["action-button"]} onClick={() => navigate(`/listing/${listing.listingId}`)}>
          {isAuction ? "To Auction" : "To Buy Now"}
        </button>

        {/* Przycisk Delete (jeśli użytkownik to sprzedawca) */}
        {isSeller && (
          <button className={classes["action-button"]} onClick={handleDelete}>
            Delete
          </button>
        )}

        {isWinner && (
          <button className={classes["action-button"]} onClick={handleBuyNow}>
            You Won! Buy Now
          </button>
        )}

        <div className={classes["button-container-right"]}>
          {[1, 2, 3, 4, 5].map((star) => (
            <FontAwesomeIcon
              key={star}
              icon={faStar}
              className={classes["star-icon"]}
              onClick={() => setShowModal(true)}
            />
          ))}
        </div>
      </div>

      {/* Modal recenzji */}
      {showModal && (
        <ReviewModal
          listingId={listing.listingId}
          onClose={() => setShowModal(false)}
        />
      )}
    </div>
  );
};

export default ListingComponent;
