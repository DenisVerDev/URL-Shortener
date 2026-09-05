(() => {
    const editorElement =
        document.getElementById("editor");

    const updateButton =
        document.getElementById("update-post-button");

    const updateButtonText =
        document.getElementById(
            "update-post-button-text");

    const updateSpinner =
        document.getElementById(
            "update-post-spinner");

    const updateStatus =
        document.getElementById(
            "update-post-status");

    if (
        editorElement === null ||
        updateButton === null ||
        updateButtonText === null ||
        updateSpinner === null ||
        updateStatus === null ||
        typeof Quill === "undefined"
    ) {
        return;
    }

    const quill = new Quill(
        editorElement,
        {
            theme: "snow"
        });

    updateButton.addEventListener(
        "click",
        async () => {
            setUpdatingState(true);
            showStatus("", false);

            const content =
                quill.root.innerHTML;

            try {
                const response = await fetch(
                    "/About/UpdateAboutPost",
                    {
                        method: "POST",
                        credentials: "same-origin",
                        headers: {
                            "Content-Type":
                                "application/x-www-form-urlencoded"
                        },
                        body: new URLSearchParams({
                            content: content
                        })
                    });

                if (!response.ok) {
                    throw new Error(
                        `Request failed with status ${response.status}.`);
                }

                showStatus(
                    "Post updated successfully.",
                    true);
            } catch (error) {
                console.error(error);

                showStatus(
                    "Could not update the post.",
                    false);
            } finally {
                setUpdatingState(false);
            }
        });

    function setUpdatingState(isUpdating) {
        updateButton.disabled = isUpdating;

        updateSpinner.classList.toggle(
            "d-none",
            !isUpdating);

        updateButtonText.textContent =
            isUpdating
                ? "Updating..."
                : "Update post";
    }

    function showStatus(
        message,
        isSuccessful) {
        updateStatus.textContent = message;

        updateStatus.classList.remove(
            "text-success",
            "text-danger");

        if (!message) {
            return;
        }

        updateStatus.classList.add(
            isSuccessful
                ? "text-success"
                : "text-danger");
    }
})();