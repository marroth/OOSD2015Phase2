﻿/*
 * Author : Geetha
 * Date: July 07, 2015
 * Usage: File used for add and modify Packages
 * 
 * */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TravelData;

namespace CMP248WorkShop2_Team6
{
    public partial class frmAddModifyPackages : Form
    {
        public frmAddModifyPackages()
        {
            InitializeComponent();
        }
        public bool addPackages;
        public Packages packages;

        //On loading the modify form loads with package data and the form title displays according to add or modify
        private void frmAddModifyPackages_Load(object sender, EventArgs e)
        {
            if (addPackages)
            {
                this.Text = "Add Package";
                
            }
            else
            {
                this.Text = "Modify Package";
                this.DisplayPackages();
            }
        }

        //Method to display packages
        private void DisplayPackages()
        {
            txtPkgAgencyCommission.Text = packages.PkgAgencyCommission.ToString();
            txtPkgBasePrice.Text = packages.PkgBasePrice.ToString();
            txtPkgDesc.Text = packages.PkgDesc.ToString();
            dtpPkgEndDate.Text = packages.PkgEndDate.ToString();
            dtpPkgStartDate.Text = packages.PkgStartDate.ToString();
            txtPkgName.Text = packages.PkgName.ToString();   
        }

        //Method to save add or modified package details
        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (IsValidData())
            {
                if (addPackages)
                {
                    packages = new Packages();
                    this.PutPackageData(packages);
                    try
                    {
                        packages.PackageId = PackagesDB.AddPackage(packages);
                        this.DialogResult = DialogResult.OK;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ex.GetType().ToString());
                    }
                }
                else    //modify
                {
                    Packages newPackages = new Packages();
                    newPackages.PackageId = packages.PackageId;
                    this.PutPackageData(newPackages);
                    try
                    {
                        if (!PackagesDB.UpdatePackage(packages, newPackages))
                        {
                            MessageBox.Show("Another user has updated or " +
                                "deleted that package.", "Database Error");
                            this.DialogResult = DialogResult.Retry;
                        }
                        else
                        {
                            packages = newPackages;
                            this.DialogResult = DialogResult.OK;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ex.GetType().ToString());
                    }
                }
            }
        }

        //Method to close the Add/Modify form
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Validate the all package fields
        private bool IsValidData()
        {
            // Check isPresent before retrieving any values.
            if (Validator.IsPresent(dtpPkgStartDate) &&
                Validator.IsPresent(dtpPkgEndDate) &&
                Validator.IsPresent(txtPkgAgencyCommission) &&
                Validator.IsPresent(txtPkgBasePrice))
            {
                DateTime pkgStartDate = (DateTime)dtpPkgStartDate.Value;
                DateTime pkgEndDate = (DateTime)dtpPkgEndDate.Value;
                decimal pkgAgencyCommission = Convert.ToDecimal(txtPkgAgencyCommission.Text);
                decimal pkgBasePrice = Convert.ToDecimal(txtPkgBasePrice.Text);

                return
                    //Validate packageName
                    Validator.IsPresent(txtPkgName) &&

                    //Validate packageDescription
                    Validator.IsPresent(txtPkgDesc) &&

                    //Validate Package Start and End Date
                    //Validator.IsPresent(dtpPkgStartDate) && Validator.IsPresent(dtpPkgEndDate) &&
                    Validator.DateCheck(pkgStartDate, pkgEndDate) &&

                    //Validate Package base price
                    //Validator.IsPresent(txtPkgBasePrice) &&
                    Validator.IsDecimal(txtPkgAgencyCommission) &&
                    Validator.IsPositive(txtPkgBasePrice) &&

                    //Validate Package Agency Commission
                    //Validator.IsPresent(txtPkgAgencyCommission) &&
                    Validator.IsDecimal(txtPkgAgencyCommission) &&
                    Validator.IsPositive(txtPkgAgencyCommission) &&
                    Validator.ValidateCommission(txtPkgAgencyCommission, "Package Agency Commission", pkgAgencyCommission, pkgBasePrice);
            }
            else
            {
                return false;
            }
        }

        //Method to save the user entered package details to the variables.
        private void PutPackageData(Packages packages)
        {
            //txtPackageId.Text = packages.PackageId.ToString();
            packages.PkgAgencyCommission = Convert.ToDecimal(txtPkgAgencyCommission.Text);
            packages.PkgBasePrice = Convert.ToDecimal(txtPkgBasePrice.Text);
            packages.PkgDesc= txtPkgDesc.Text;
            packages.PkgEndDate = (DateTime)dtpPkgEndDate.Value;
            packages.PkgStartDate = (DateTime)dtpPkgStartDate.Value;
            packages.PkgName = txtPkgName.Text;
        }

    }
}
