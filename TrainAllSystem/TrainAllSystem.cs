using AirportCEOModLoader.Core;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AirportCEOStaffImprovements.TrainAllSystem;

[HarmonyPatch]
public static class TrainAllSystem
{
    static Transform staffinteractionbar;
    static Transform originalHappinessTransform;
    static Transform newHappinessTransform;
    static Button newHappinessButton;
    static TextMeshProUGUI newHappinessText;

    private static EmployeePanelUI panelUI;

    [HarmonyPatch(typeof(EmployeePanelUI), nameof(EmployeePanelUI.InitializePanel))]
    [HarmonyPostfix]
    public static void CreateButton(EmployeePanelUI __instance)
    {
        panelUI = __instance;

        try
        {
            staffinteractionbar = ManagementPanelController.Instance.interactionBarTransforms.Find("StaffInteractionBar");
            originalHappinessTransform = staffinteractionbar.Find("Happiness");

            newHappinessTransform = GameObject.Instantiate(originalHappinessTransform, staffinteractionbar);
            newHappinessTransform.SetSiblingIndex(1);
            newHappinessTransform.gameObject.SetActive(true);
            newHappinessText = newHappinessTransform?.GetChild(0)?.GetComponent<TextMeshProUGUI>();

            newHappinessText.text = "Train All Staff";

            newHappinessButton = newHappinessTransform.gameObject.AddComponent<Button>();
            ColorBlock colorBlock = new()
            {
                normalColor = DataPlaceholderColors.Instance.lightBlue,
                highlightedColor = DataPlaceholderColors.Instance.blue,
                colorMultiplier = 1,
                disabledColor = DataPlaceholderColors.Instance.gray,
                pressedColor = DataPlaceholderColors.Instance.darkBlue,
                selectedColor = DataPlaceholderColors.Instance.lightBlue,
                fadeDuration = 0.1f,
            };
            newHappinessButton.colors = colorBlock;
            newHappinessButton.onClick.AddListener(OnTrainAllClick);

            Image image = newHappinessTransform.GetComponent<Image>();
            image.enabled = true;
            image.color = Color.white;

            newHappinessButton.enabled = true;

            AirportCEOStaffImprovements.SILogger.LogInfo("Completed button creation?");
        }
        catch (Exception ex)
        {
            AirportCEOStaffImprovements.SILogger.LogError($"Failed to create new icon. {ExceptionUtils.ProccessException(ex)}");
        }
    }

    public static void OnTrainAllClick()
    {
        if (!AirportController.Instance.HasHiredEmployeeType(Enums.EmployeeType.HRDirector))
        {
            DialogUtils.QueueDialog("An HR Director is required to mass train staff.");
            return;
        }

        float estimatedCost = 0;
        foreach (EmployeeController employee in GetTrainableEmployees())
        {
            estimatedCost += employee.TrainingPrice;
        }

        if (estimatedCost == 0)
        {
            DialogUtils.QueueDialog("All staff are fully trained!");
            return;
        }

        DialogPanel.Instance.ShowQuestionPanel(TrainAllStaffOnce, $"Train all staff (that can be trained) once? Estimated cost: {Utils.GetCurrencyFormat(estimatedCost)}", true, false);
    }

    public static void TrainAllStaffOnce(bool questionResult)
    {
        if (!questionResult)
        {
            return;
        }

        foreach (EmployeeController employee in GetTrainableEmployees())
        {
            employee.TrainEmployee();
        }
        panelUI.GenerateEmployeeContainers();
    }

    private static IEnumerable<EmployeeController> GetTrainableEmployees()
    {
        return AirportController.Instance.allEmployees.Where(emp => emp.CanTrain && emp.employeeModel.isHired && !emp.employeeModel.isFired);
    }
}
