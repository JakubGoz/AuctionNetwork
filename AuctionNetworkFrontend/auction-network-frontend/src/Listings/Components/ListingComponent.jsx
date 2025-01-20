import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faStar } from '@fortawesome/free-solid-svg-icons';
import classes from './ListingComponent.module.scss';
import ReviewModal from './ReviewModal'; // Import modala recenzji

const ListingComponent = ({ listing }) => {
  const isAuction = listing.isAuction;
  const navigate = useNavigate();
  const [showModal, setShowModal] = useState(false); // Stan modala

  // Funkcja nawigacji do szczegółów aukcji
  const handleNavigation = () => {
    navigate(`/listing/${listing.listingId}`); // Używamy poprawnego ID
  };

  return (
    <div className={`${classes['listing-item']} d-flex flex-wrap`}>
      <div className={`${classes['left-container']} flex-grow-1`}>
        <div>
          <strong>Title:</strong> {listing.title}
        </div>
        <div>
          <strong>Status:</strong> {listing.listingStatus}
        </div>
        <div>
          <strong>Start Date:</strong> {new Date(listing.startDate).toLocaleDateString()}
        </div>
        {listing.endDate && (
          <div>
            <strong>End Date:</strong> {new Date(listing.endDate).toLocaleString()}
          </div>
        )}
        <div>
          <strong>User Reviews Count:</strong> {listing.userReviewsCount}
        </div>
      </div>

      <div className={`${classes['right-container']} flex-grow-1`}>
        <div>
          <strong>Seller:</strong> {listing.sellerUserName}
        </div>
        <div>
          <strong>Price:</strong> {listing.price} PLN
        </div>

        <button
          className={classes['action-button']}
          onClick={handleNavigation} // Funkcja nawigacji
        >
          {isAuction ? 'To Auction' : 'To Buy Now'}
        </button>

        <div className={classes['button-container-right']}>
          {/* Ikony gwiazdek */}
          {[1, 2, 3, 4, 5].map((star) => (
            <FontAwesomeIcon
              key={star}
              icon={faStar}
              className={classes['star-icon']}
              onClick={() => setShowModal(true)} // Otwórz modal po kliknięciu
            />
          ))}
        </div>
      </div>

      {/* Modal recenzji */}
      {showModal && (
        <ReviewModal
          listingId={listing.listingId} // Używamy poprawnego identyfikatora
          onClose={() => setShowModal(false)} // Zamknij modal
        />
      )}
    </div>
  );
};

export default ListingComponent;
