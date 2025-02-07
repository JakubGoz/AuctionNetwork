import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';
import { baseUrl } from '../../Shared/Options/ApiOptions';
import classes from './ListingDetails.module.scss';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faThumbsUp, faThumbsDown } from '@fortawesome/free-solid-svg-icons';
import ReviewsModal from './ReviewsModal';
import noImageAvailable from '../../Shared/No_Image_Available.jpg'; // Import obrazka


const ListingDetails = () => {
  const { id } = useParams();
  const [listing, setListing] = useState(null);
  const [photoUrl, setPhotoUrl] = useState(null); 
  const [userInfo, setUserInfo] = useState(null); 
  const [loading, setLoading] = useState(true); // Stan ładowania zdjęcia
  const [showReviewsModal, setShowReviewsModal] = useState(false);
  const [bidAmount, setBidAmount] = useState(''); // Pole na kwotę
  const [bids, setBids] = useState([]); // Stan na listę ofert
  const [showBidsModal, setShowBidsModal] = useState(false); // Modal dla listy bidów
  const navigate = useNavigate();

  useEffect(() => {
    const fetchListing = async () => {
      try {
        const token = localStorage.getItem('token');
        const [listingRes, userRes] = await Promise.all([
          axios.get(`${baseUrl}/listing/${id}`, { headers: { Authorization: `Bearer ${token}` } }),
          axios.get(`${baseUrl}/user/user-short-info`, { headers: { Authorization: `Bearer ${token}` } })
        ]);
        setListing(listingRes.data);
        setUserInfo(userRes.data);
        
        // Pobieranie zdjęcia
        const fetchPhoto = async () => {
          try {
            const photoResponse = await fetch(`${baseUrl}/listing/${id}/listing-picture`, {
              method: 'GET',
              headers: {
                Authorization: `Bearer ${token}`,
              },
            });

            if (photoResponse.ok) {
              const blob = await photoResponse.blob();
              const url = URL.createObjectURL(blob);
              setPhotoUrl(url);
            } else {
              console.error('Failed to fetch the listing picture.');
            }
          } catch (error) {
            console.error('Error fetching listing picture:', error);
          } finally {
            setLoading(false);
          }
        };

        fetchPhoto();
      } catch (error) {
        console.error('Error fetching listing:', error);
      }
    };

    fetchListing();
  }, [id]);

  const handleReviewsButtonClick = () => {
    setShowReviewsModal(true);
  };

  

  const handleLikeDislike = async (thumbUp) => {
    try {
      const token = localStorage.getItem('token');
      await axios.put(
        `${baseUrl}/user/${listing.sellerId}/toggle-like/${thumbUp}`,
        null, // Brak treści w body
        { headers: { Authorization: `Bearer ${token}` } }
      );
      window.location.reload();
      alert('Review successfully added!');
    } catch (error) {
      console.error('Error toggling like/dislike:', error);
      alert('An error occurred while adding the review. Please try again.');
    }
  };
  const handleDelete = async () => {
    if (window.confirm("Are you sure you want to delete this listing?")) {
      try {
        const token = localStorage.getItem("token");
        await axios.delete(`${baseUrl}/listing/${id}`, {
          headers: { Authorization: `Bearer ${token}` },
        });
        alert("Listing deleted successfully!");
        // Przekierowanie użytkownika po usunięciu listingu
        window.location.href = "/home";
      } catch (error) {
        console.error("Error deleting listing:", error);
        alert("An error occurred while deleting the listing.");
      }
    }
  };
  const handleBuyNow = () => {
    navigate(`/order-payment/${id}`);
  };

  const handleUpdate = () => {
    navigate(`/create-listing/${id}`);
  };
  
  
  const handlePlaceBid = async () => {
    const bidValue = parseFloat(bidAmount);
    
    // Walidacja danych wejściowych
    if (!bidAmount || isNaN(bidValue)) {
      alert('Please enter a valid bid amount.');
      return;
    }
    
    if (bidValue <= listing.price) {
      alert('Bid must be greater than the current price by at least 1 PLN.');
      return;
    }
  
    if (listing.buyNowPrice && bidValue > listing.buyNowPrice) {
      alert('Bid cannot exceed the Buy Now price.');
      return;
  }
  
    try {
      const token = localStorage.getItem('token');
      const request = { ListingId: id, Price: bidValue };
      await axios.post(`${baseUrl}/bid/add/${id}`, request, {
        headers: { Authorization: `Bearer ${token}` },
      });
  
      alert('Bid placed successfully!');
      window.location.reload();
    } catch (error) {
      console.error('Error placing bid:', error);
      alert('An error occurred while placing the bid.');
    }
  };
  

  const handleFetchBids = async () => {
    try {
      const token = localStorage.getItem('token');
  
      const response = await axios.get(`${baseUrl}/bid/listing/${id}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
  
      setBids(response.data);
      setShowBidsModal(true);
    } catch (error) {
      console.error('Error fetching bids:', error);
      alert('An error occurred while fetching bids.');
    }
  };
  
  const handleUserListingsClick = () => {
    window.location.href = `/listing/user/${listing.sellerId}`;
  };

  if (!listing || !userInfo) {
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
    sellerReviewsCount,
    sellerApprovePercentage,
    winnerId
  } = listing;
  const { userId } = userInfo;
  return (
    <div className={classes.container}>
      <div className={classes.main}>
        {/* Image container */}
        <div className={classes.imageContainer}>
  {loading ? (
    <p>Loading image...</p>
  ) : photoUrl ? (
    <img
      src={photoUrl}
      alt={`Listing ${title}`}
      className={classes.image}
    />
  ) : (
    <img
    src={noImageAvailable} 
    alt="No image available"
    className={classes.image}
  />
  )}
</div>

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
            <p>{isAuction ? 'Last Bid' : 'Price'}: {price} PLN</p>
            {buyNowPrice && <p>Buy Now Price: {buyNowPrice} PLN</p>}
          </div>
          <div className={classes.buyButtons}>
  {isItMyListing ? (
    <>
<button className={classes.bidButton} onClick={handleUpdate}>Update</button>
<button className={classes.bidButton} onClick={handleDelete}>Delete</button>
    </>
  ) : (
    <>
      {listingStatus === 'Sold' ? (
        <p className={classes.soldMessage}>This item has been sold.</p>
      ) : listingStatus === 'Ended' && winnerId === userId ? (
        <>
          <p className={classes.winnerMessage}>You won the auction! Proceed to place your order.</p>
          <button className={classes.bidButton} onClick={handleBuyNow}>
            Buy Now
          </button>
        </>
      ) : listingStatus === 'Ended' && winnerId !== userId ? (
        <p className={classes.lostAuctionMessage}>Unfortunately, you did not win this auction.</p>
      ) : (
        <>
          {isAuction && (
            <div className={classes['button-container']}>
              <input
                type="number"
                className={classes.bidInput}
                placeholder="Enter bid"
                value={bidAmount}
                onChange={(e) => setBidAmount(e.target.value)}
              />
              <button className={classes.bidButton} onClick={handlePlaceBid}>
                Place Bid
              </button>
              {buyNowPrice && (
                <button className={classes.bidButton} onClick={handleBuyNow}>
                Buy Now
              </button>
              )}
              <button className={classes.bidButton} onClick={handleFetchBids}>
                Bids
              </button>
            </div>
          )}
          {!isAuction && (
            <button className={classes.bidButton} onClick={handleBuyNow}>
              Buy Now
            </button>
          )}
        </>
      )}
    </>
  )}
</div>
        </div>

        <div className={classes.topRight}>
          {isItMyListing ? (
            <p>Your Listing</p>
          ) : (
            <>
              <p>Seller: {sellerUserName}</p>
              <p>Email: {sellerEmail}</p>
              {sellerPhoneNumber && <p>Phone: {sellerPhoneNumber}</p>}
              <p>Seller Reviews count: {sellerReviewsCount}</p>
              <p>Approval: {sellerApprovePercentage}%</p>
              <div className={classes['button-container']}>
                <button
                  className={classes['like-button']}
                  onClick={() => handleLikeDislike(true)}
                >
                  <FontAwesomeIcon icon={faThumbsUp} />
                </button>
                <button
                  className={classes['dislike-button']}
                  onClick={() => handleLikeDislike(false)}
                >
                  <FontAwesomeIcon icon={faThumbsDown} />
                </button>
              </div>
              <button
                  className={classes.otherListingsButton}
                  onClick={handleUserListingsClick}
              >
               User's Other Listings
              </button>
            </>
          )}
        </div>
      </div>

      <div className={classes.description}>
        <h2>Description</h2>
        <p>{listing.description}</p>
      </div>

      {showReviewsModal && (
        <ReviewsModal
          listingId={id}
          onClose={() => setShowReviewsModal(false)}
        />
      )}
      {showBidsModal && (
  <div className={classes.bidsModal}>
    <h2>Bids</h2>
    <div className={classes.bidsContainer}>
      <ul>
        {bids.map((bid, index) => (
          <li key={index}>
            <p><strong>User:</strong> {bid.userUserName}</p>
            <p><strong>Price:</strong> {bid.price} PLN</p>
            <p><strong>Date:</strong> {new Date(bid.bidDate).toLocaleString()}</p>
          </li>
        ))}
      </ul>
    </div>
    <button
      className={classes.buyNowButton}
      onClick={() => setShowBidsModal(false)}
    >
      Close
    </button>
  </div>
  )}

    </div>
  );
};

export default ListingDetails;