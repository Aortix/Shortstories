﻿const pageLoadFunction = () => {
    //Home Page
    const homeInstance = new Home();
    if (homeInstance.homePage !== null) {
        homeInstance.getStoriesByFollowers(window.localStorage.getItem("pid"), "home-follower-stories");
        homeInstance.getStoriesByFilter("New", "home-filter-stories");
        homeInstance.addMainDropdownButtonEventListener();
        homeInstance.addAllDropdownButtonEventListeners();
    }

    //Register Page
    const registerInstance = new Register();
    registerInstance.addFormEventListener();
    registerInstance.changeChosenAvatar();

    //Login Page
    const loginInstance = new Login();
    loginInstance.addFormEventListener();

    //Header Component
    const headerInstance = new Header();
    headerInstance.addClickFunctionalityToHeaderHamburgerIcon();
    headerInstance.checkAuthorizationState().then((username) => {
        if (username) {
            document.getElementsByClassName("header-unauthenticated")[0].style.display = "none";
            document.getElementById("header-username").innerHTML = username;
            document.getElementsByClassName("header-authenticated")[0].style.display = "block";
            headerInstance.logoutEventListener();
            headerInstance.setAvatar();
            headerInstance.setProfileLink();
            headerInstance.setSettingsLink();
        }
    }).catch((err) => {
    });

    //Profile Page
    const profileInstance = new Profile();
    if (profileInstance.profileViewId != null) {
        if (window.localStorage.getItem("pid") !== null) {
            profileInstance.followButtonAddEventListeners();
            profileInstance.unfollowButtonAddEventListners();
            profileInstance.checkIfUserIsAFriend();
        } else {
            profileInstance.hideButtons();
        }
        profileInstance.displayProfileAvatar();
        homeInstance.getStoriesByProfile(document.getElementById("profile-username").innerHTML, "profile-stories");
    }

    //Story Page
    const storyInstance = new Story();

    //Edit Story Page
    const editStoryInstance = new EditStory();
    if (editStoryInstance.editStoryPage !== null) {
        editStoryInstance.getStoryDetails(document.getElementById("edit-story-id").value).then(() => {
            editStoryInstance.updateStoryButtonFunctionality();
        }).catch((err) => {
        })
        editStoryInstance.updateSelectedGenresFunctionality();
        editStoryInstance.switchingBetweenStoryAndChapterSelection();
    }

    //Settings Page
    const settingsInstance = new Settings();
    document.getElementById("main-container").addEventListener("click", (e) => {
        if (e.target.id !== "settings-update-user-button") {
            settingsInstance.hideCurrentLoginDetailsPrompt();
        }
    })

    if (settingsInstance.settingsPage !== null) {
        settingsInstance.preSelectCurrentAvatar();
        settingsInstance.preSelectCurrentWriterLabel();
        settingsInstance.preFillProfileDescription();
        settingsInstance.displayCurrentStories();
        settingsInstance.updateUserButtonEventListeners();
        settingsInstance.updateProfileButtonEventListeners();
    }
};

window.addEventListener('DOMContentLoaded', () => {
    pageLoadFunction();
});