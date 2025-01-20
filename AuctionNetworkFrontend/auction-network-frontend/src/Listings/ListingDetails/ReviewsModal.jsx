import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { baseUrl } from '../../Shared/Options/ApiOptions';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faStar } from '@fortawesome/free-solid-svg-icons';
import classes from './ReviewsModal.module.scss';

const ReviewsModal = ({ listingId, onClose }) => {
  const [reviews, setReviews] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchReviews = async () => {
      try {
        const token = localStorage.getItem('token');
        const response = await axios.get(`${baseUrl}/listing/${listingId}/getreviews`, {
          headers: { Authorization: `Bearer ${token}` },
        });
        setReviews(response.data); // Załaduj recenzje
      } catch (error) {
        setError('Failed to fetch reviews. Please try again.');
      }
    };

    fetchReviews();
  }, [listingId]);

  return (
    <div className={classes.modalContainer}>
      <div className={classes.modalContent}>
        <h3>User Reviews</h3>
        {error && <p className={classes.error}>{error}</p>}

        <div className={classes.reviewsList}>
          {reviews.length > 0 ? (
            reviews.map((review) => (
              <div key={review.id} className={classes.review}>
                <div className={classes.reviewHeader}>
                  <span className={classes.username}>{review.reviewerUserName}</span>
                  <div className={classes.rating}>
                    {[1, 2, 3, 4, 5].map((star) => (
                      <FontAwesomeIcon
                        key={star}
                        icon={faStar}
                        className={star <= review.rating ? classes.selected : classes.unselected}
                      />
                    ))}
                  </div>
                </div>
                {review.description && <p className={classes.description}>{review.description}</p>}
              </div>
            ))
          ) : (
            <p>No reviews available for this listing.</p>
          )}
        </div>

        <button className={classes.closeButton} onClick={onClose}>
          Close
        </button>
      </div>
    </div>
  );
};

export default ReviewsModal;
