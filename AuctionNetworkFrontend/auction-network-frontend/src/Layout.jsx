import React, { useEffect, useState } from 'react';
import { Link, Outlet, useNavigate } from 'react-router-dom';
import classes from './Layout.module.scss';
import { baseUrl, authorization } from './Shared/Options/ApiOptions';
import axios from 'axios';
import { faSearch, faRightFromBracket, faCaretDown, faHome  } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

const Layout = ({ isAuthenticated }) => {
    const [userData, setUserData] = useState(null);
    const [categories, setCategories] = useState([]);
    const [dropdownOpen, setDropdownOpen] = useState(false);
    const [categoriesDropdownOpen, setCategoriesDropdownOpen] = useState(false);
    

    const [filters, setFilters] = useState({
        searchQuery: '',
        minPrice: '',
        maxPrice: '',
        isAuction: null,
        categoryId: '',
        orderBy: 1,
    });
    const [filterVisible, setFilterVisible] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        const getUserShortInfo = async () => {
            if (!isAuthenticated) {
                navigate('/auth');
                return;
            }

            try {
                const response = await axios.get(
                    `${baseUrl}/user/user-short-info`,
                    authorization(localStorage.getItem("token"))
                );
                setUserData(response.data);
            } catch (err) {
                alert(err.response.data);
            }
        };

        const fetchCategories = async () => {
            try {
                const response = await axios.get(`${baseUrl}/category`, authorization(localStorage.getItem("token")));
                setCategories(response.data);
            } catch (err) {
                console.error("Error fetching categories", err);
            }
        };

        getUserShortInfo();
        fetchCategories();
    }, []);

    useEffect(() => {
        const handleOutsideClick = (event) => {
            if (!event.target.closest(`.${classes.dropdown}`)) {
                setDropdownOpen(false);
                setCategoriesDropdownOpen(false);
            }
            
        };
    
        if (dropdownOpen || categoriesDropdownOpen || filterVisible) {
            document.addEventListener('click', handleOutsideClick);
        }
    
        return () => {
            document.removeEventListener('click', handleOutsideClick);
        };
    }, [dropdownOpen, categoriesDropdownOpen, filterVisible]);

    const handleLogOut = () => {
        localStorage.removeItem("token");
        window.location.reload();
    };

    const handleFilterChange = (key, value) => {
        setFilters((prevFilters) => ({ ...prevFilters, [key]: value }));
    };

    const handleSearch = () => {
        navigate('/listing/results', { replace: true, state: { filters } });
    };

    useEffect(() => {
        setFilterVisible(false); // Ukryj filtry po zmianie ścieżki
    }, [location.pathname]);

    const handleDropdownToggle = () => {
        setDropdownOpen((prevState) => !prevState);
    };

    const handleCategoriesDropdownToggle = () => {
        setCategoriesDropdownOpen((prevState) => !prevState);
    };

    const handleCategoryClick = (path) => {
        
        setCategoriesDropdownOpen(false);
        navigate(path);
    };

    const handleDropdownClick = (path) => {
        setDropdownOpen(false); // Zamknij listę rozwijaną po kliknięciu
        navigate(path);
        window.location.reload();

    };

    if (!userData) {
        return <div>Loading...</div>;
    }

    return (
        <div className={classes.container}>
            <header className={`${classes.header} d-flex align-items-center justify-content-between`}>
                <div className='d-flex align-items-center'>
                    <button onClick={() => navigate('/')} className={`${classes["home-button"]} me-3`}>
                        <FontAwesomeIcon icon={faHome} />
                    </button>
                    <div className='m-0'>Auction Network</div>
                    <div className={`${classes.dropdown} ms-3`}>
                        <button
                            onClick={handleCategoriesDropdownToggle}
                            className={`${classes["dropdown-button"]} d-flex align-items-center`}
                        >
                            Categories
                            <FontAwesomeIcon icon={faCaretDown} className={`${classes.icon} ms-2`} />
                        </button>
                        {categoriesDropdownOpen && (
                            <div className={`${classes["dropdown-menu"]}`}>
                                {categories.map((category) => (
                                    <button
                                        key={category.id}
                                        onClick={() => handleCategoryClick(`/categories/${category.id}`)}
                                        className={`${classes["dropdown-item"]}`}
                                    >
                                        {category.name}
                                    </button>
                                ))}
                            </div>
                        )}
                    </div>
                </div>

                <div className="search-container ms-3">
                    <input
                        type="text"
                        value={filters.searchQuery}
                        onChange={(e) => handleFilterChange('searchQuery', e.target.value)}
                        placeholder="Search for listings..."
                    />
                    <button onClick={handleSearch} className={classes["filter-button"]}>
                        <FontAwesomeIcon icon={faSearch} />
                    </button>
                    <button
                        onClick={() => setFilterVisible(!filterVisible)}
                        className={classes["filter-button"]}
                    >Filters</button>
                </div>

                <div className='m-0 d-flex g-2 align-items-center'>
                    <button
                        onClick={() => navigate('/CreateListing')}
                        className={`${classes["create-listing-button"]} ms-3`}
                    >
                        Create Listing
                    </button>
                    <div className={`${classes.dropdown} ms-3`}>
                        <button
                            onClick={handleDropdownToggle}
                            className={`${classes["dropdown-button"]} d-flex align-items-center`}
                        >
                            My Items
                            <FontAwesomeIcon icon={faCaretDown} className={`${classes.icon} ms-2`} />
                        </button>
                        {dropdownOpen && (
                            <div className={`${classes["dropdown-menu"]}`}>
                                <button
                                    onClick={() => handleDropdownClick('/listing/sales')}
                                    className={`${classes["dropdown-item"]}`}
                                >
                                    Sales
                                </button>
                                <button
                                    onClick={() => handleDropdownClick('/listing/purchases')}
                                    className={`${classes["dropdown-item"]}`}
                                >
                                    Purchases
                                </button>
                                <button
                                    onClick={() => handleDropdownClick('/bid/myitems')}
                                    className={`${classes["dropdown-item"]}`}
                                >
                                    Bids
                                </button>
                            </div>
                        )}
                    </div>

                    <div className='ms-2'>{userData.userName}</div>
                    <button onClick={handleLogOut} className='ms-3'>
                        <FontAwesomeIcon icon={faRightFromBracket} className={`${classes.icon} ${classes["log-out-button"]}`} />
                    </button>
                </div>
            </header>

            {filterVisible && (
                <div className={classes.filter}>
                    
                    <div className={classes.left}>
                        <label>Min Price:</label>
                        <input
                            type="number"
                            value={filters.minPrice}
                            onChange={(e) => handleFilterChange('minPrice', e.target.value)}
                            placeholder="Min Price"
                        />
                        <label>Max Price:</label>
                        <input
                            type="number"
                            value={filters.maxPrice}
                            onChange={(e) => handleFilterChange('maxPrice', e.target.value)}
                            placeholder="Max Price"
                        />
                    </div>
                    
                    <div className={classes.center}>
                        <label>Category:</label>
                        <select value={filters.categoryId} 
                        onChange={(e) => handleFilterChange('categoryId', e.target.value)}
                        >
                            <option value="">Select a Category</option>
                            {categories.map((category) => (
                                <option key={category.id} value={category.id}>
                                    {category.name}
                                </option>
                            ))}
                        </select>
                        <label>
                            Auction:
                            <input
                                type="checkbox"
                                checked={filters.isAuction || false}
                                onChange={(e) => handleFilterChange('isAuction', e.target.checked)}
                            />
                        </label>
                    
                    </div>
                    <div className={classes.right}>
                        <label>Sort By:</label>
                        <select value={filters.orderBy} 
                        onChange={(e) => handleFilterChange('orderBy', e.target.value)}
                        >
                            <option value={1}>Price: Low to High</option>
                            <option value={2}>Price: High to Low</option>
                            <option value={3}>Best Rated</option>
                            <option value={4}>Newest Listings</option>
                            <option value={5}>Ending Soon</option>
                        </select>
                        <button className= {classes["filter-button"]} onClick={handleSearch}>Apply Filter</button>
                    </div>
                    
                </div>
            )}

            <main className={classes.content}>
                <Outlet context={{ filters }}/>
            </main>
        </div>
    );
};

export default Layout;
