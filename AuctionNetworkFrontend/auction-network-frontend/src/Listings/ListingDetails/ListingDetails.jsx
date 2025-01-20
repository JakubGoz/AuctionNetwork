import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';
import { baseUrl } from '../../Shared/Options/ApiOptions';
import classes from './ListingDetails.module.scss';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faThumbsUp, faThumbsDown } from '@fortawesome/free-solid-svg-icons';
import ReviewsModal from './ReviewsModal'; // Importujemy komponent modala

const ListingDetails = () => {
  const { id } = useParams();
  const [listing, setListing] = useState(null);
  const [showReviewsModal, setShowReviewsModal] = useState(false); // Stan do zarządzania widocznością modala z recenzjami

  useEffect(() => {
    const fetchListing = async () => {
      try {
        const token = localStorage.getItem('token');
        const response = await axios.get(`${baseUrl}/listing/${id}`, {
          headers: { Authorization: `Bearer ${token}` },
        });
        setListing(response.data);
      } catch (error) {
        console.error('Error fetching listing:', error);
      }
    };
    fetchListing();
  }, [id]);

  const handleReviewsButtonClick = () => {
    setShowReviewsModal(true); // Pokazujemy modal po kliknięciu przycisku
  };

  if (!listing) {
    return <div>Loading...</div>;
  }

  const {
    title,
    listingStatus,
    startDate,
    endDate,
    price,
    buyNowPrice,
    isAuction,
    isItMyListing,
    sellerUserName,
    sellerEmail,
    sellerPhoneNumber,
    listingReviewsCount,
  } = listing;

  return (
    <div className={classes.container}>
      {/* Główny kontener */}
      <div className={classes.main}>
        {/* Pierwszy kontener */}
        <div className={classes.topLeft}>
          <h1>{title}</h1>
          <p>Status: {listingStatus}</p>
          <div className={classes.dateContainer}>
            <p>Start Date: {new Date(startDate).toLocaleDateString()}</p>
            {isAuction && <p>End Date: {new Date(endDate).toLocaleString()}</p>}
          </div>
          
          <button className={classes.reviewsButton} onClick={handleReviewsButtonClick}>
            Reviews: {listingReviewsCount}
          </button>

          <div className={classes.priceContainer}>
            <p>Price: {isAuction ? 'Last Bid' : 'Price'} {price} PLN</p>
            {buyNowPrice && <p>Buy Now Price: {buyNowPrice} PLN</p>}
          </div>
          <div className={classes.buyButtons}>
            {isAuction && (
              <>
                <div className={classes['button-container']}>
                  <button className={classes.bidButton}>Place Bid</button>
                  {buyNowPrice && (
                    <button className={classes.buyNowButton}>Buy Now</button>
                  )}
                </div>
              </>
            )}
            {!isAuction && (
              <button className={classes.buyNowButton}>Buy Now</button>
            )}
          </div>
        </div>
        
        
        {/* Drugi kontener */}
        <div className={classes.topRight}>
          {isItMyListing ? (
            <>
              <p>Your Listing</p>
            </>
          ) : (
            <>
              <p>Seller: {sellerUserName}</p>
              <p>Email: {sellerEmail}</p>
              {sellerPhoneNumber && <p>Phone: {sellerPhoneNumber}</p>}
            </>
          )}
        </div>
      </div>

      {/* Opis */}
      <div className={classes.description}>
        <h2>Description</h2>
        <p>{listing.description}</p>
      </div>

      {/* Modal z recenzjami */}
      {showReviewsModal && (
        <ReviewsModal
          listingId={id}
          onClose={() => setShowReviewsModal(false)} // Zamykamy modal
        />
      )}
    </div>
  );
};

export default ListingDetails;
